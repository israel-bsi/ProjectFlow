using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProjectFlow.ApiService.Data;
using ProjectFlow.ApiService.Services;
using ProjectFlow.Core.Enums;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Projects.Request;
using ProjectFlow.Core.Models.DTOs.Projects.Response;
using ProjectFlow.Core.Models.Entities;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Handlers;

public class ProjectHandler : IProjectHandler
{
    private readonly AppDbContext _context;
    private readonly IAppSettingsHandler _appSettingsHandler;
    private readonly ILogger<ProjectHandler> _logger;

    public ProjectHandler(AppDbContext context,
        IAppSettingsHandler appSettingsHandler, 
        ILogger<ProjectHandler> logger)
    {
        _context = context;
        _appSettingsHandler = appSettingsHandler;
        _logger = logger;
    }

    public async Task<Response<ProjectResponse>> CreateAsync(ProjectRequest request)
    {
        var user = await _context.Users.FindAsync(request.User.Id);
        if (user is null)
            return new Response<ProjectResponse>(null, 404, "Usuário não encontrado");

        var customer = await _context.Customers.FindAsync(request.Customer.Id);
        if (customer is null)
            return new Response<ProjectResponse>(null, 404, "Cliente não encontrado");

        var project = request.ToProject();
        project.User = user;
        project.Customer = customer;

        foreach (var developer in project.Developers)
            _context.Attach(developer);

        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Projeto {ProjectId} criado pelo usuário {UserName}", 
            project.Id, user.GivenName);

        var response = project.ToProjectResponse();

        return new Response<ProjectResponse>(response, 201);
    }

    public async Task<Response<ProjectResponse>> UpdateAsync(ProjectRequest request)
    {
        try
        {
            var project = await _context
                .Projects
                .Include(p => p.Developers)
                .Include(p => p.ProjectServices)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive);

            if (project is null)
                return new Response<ProjectResponse>(null, 404, "Projeto não encontrado");

            var user = await _context.Users.FindAsync(request.User.Id);
            if (user is null)
                return new Response<ProjectResponse>(null, 404, "Usuário não encontrado");

            var customer = await _context.Customers.FindAsync(request.Customer.Id);
            if (customer is null)
                return new Response<ProjectResponse>(null, 404, "Cliente não encontrado");

            project.UpdatedAt = DateTime.Now.ToUnspecifiedKind();
            project.Title = request.Title;
            project.Description = request.Description;
            project.TotalHours = request.TotalHours;
            project.Requester = request.Requester;
            project.TotalValue = request.TotalValue;
            project.DiscountType = request.DiscountType;
            project.DiscountValue = request.DiscountValue;
            project.Deadline = request.Deadline;
            project.DaysToAddToDeadline = request.DaysToAddToDeadline;
            project.ProjectStatus = request.ProjectStatus;
            project.PaymentStatus = request.PaymentStatus;
            project.RequestedAt = request.RequestedAt ?? DateTime.Now;
            project.User = user;
            project.Customer = customer;

            UpdateDevelopers(project, request);
            UpdateServices(project, request);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Projeto {ProjectId} atualizado pelo usuário {UserName}", 
                project.Id, user.GivenName);

            var response = project.ToProjectResponse();

            return new Response<ProjectResponse>(response);
        }
        catch (Exception e)
        {
            return new Response<ProjectResponse>(null, 500, e.Message);
        }
    }

    private void UpdateDevelopers(Project project, ProjectRequest request)
    {
        var requestedDeveloperIds = request.Developers.Select(d => d.Id).ToList();

        var developersToRemove = project.Developers
            .Where(d => !requestedDeveloperIds.Contains(d.Id))
            .ToList();

        foreach (var developer in developersToRemove) 
            project.Developers.Remove(developer);

        foreach (var developerRequest in request.Developers)
        {
            var existingDeveloper = project.Developers
                .FirstOrDefault(d => d.Id == developerRequest.Id);

            if (existingDeveloper != null)
            {
                existingDeveloper.UpdateEntity(developerRequest);
            }
            else
            {
                var newDeveloper = developerRequest.ToDeveloper();
                project.Developers.Add(newDeveloper);
            }
        }
    }

    private void UpdateServices(Project project, ProjectRequest request)
    {
        var requestedServicesIds = request.ProjectServices.Select(d => d.Id).ToList();

        var servicesToRemove = project.ProjectServices
            .Where(d => !requestedServicesIds.Contains(d.Id))
            .ToList();

        foreach (var service in servicesToRemove) 
            project.ProjectServices.Remove(service);

        foreach (var serviceRequest in request.ProjectServices)
        {
            var existingService = project.ProjectServices
                .FirstOrDefault(d => d.Id == serviceRequest.Id);

            if (existingService != null)
            {
                existingService.UpdateEntity(serviceRequest);
            }
            else
            {
                var newService = serviceRequest.ToProjectService();
                project.ProjectServices.Add(newService);
            }
        }
    }

    public async Task<Response<ProjectResponse>> DeleteAsync(DeleteProjectRequest request)
    {
        var user = await _context.Users.FindAsync(request.User.Id);
        if (user is null)
            return new Response<ProjectResponse>(null, 404, "Usuário não encontrado");

        var project = await _context
            .Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive);

        if (project is null)
            return new Response<ProjectResponse>(null, 404, "Projeto não encontrado");

        project.IsActive = false;
        project.UpdatedAt = DateTime.Now.ToUnspecifiedKind();

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Projeto {ProjectId} excluído pelo usuário {UserName}",
            project.Id, user.GivenName);

        return new Response<ProjectResponse>(null, 204);
    }

    public async Task<Response<ProjectResponse>> GetByIdAsync(GetProjectByIdRequest request)
    {
        var project = await _context
            .Projects
            .AsNoTracking()
            .Include(p => p.ProjectServices)
            .Include(p => p.Developers)
            .Include(p => p.Customer)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive);

        var response = project?.ToProjectResponse();

        return response is null
            ? new Response<ProjectResponse>(null, 404, "Projeto não encontrado")
            : new Response<ProjectResponse>(response);
    }

    public async Task<PagedResponse<List<ProjectResponse>>> GetAllAsync(GetAllProjectsRequest request)
    {
        var query = _context
            .Projects
            .AsNoTracking()
            .Include(p => p.ProjectServices)
            .Include(p => p.Developers)
            .Include(p => p.Customer)
            .Include(p => p.User)
            .Where(p => p.IsActive);

        if (!string.IsNullOrEmpty(request.FilterBy))
            query = query.FilterByProperty(request.SearchTerm, request.FilterBy);

        var projects = await query
            .OrderByDescending(p => p.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var count = await query.CountAsync();

        var response = projects.Select(p => p.ToProjectResponse()).ToList();

        return new PagedResponse<List<ProjectResponse>>(response, count, request.PageNumber, request.PageSize);
    }

    public async Task<Response<string>> UpdateStatusAsync(UpdateStatusProjectRequest request)
    {
        var project = await _context
            .Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive);

        if (project is null)
            return new Response<string>(null, 404, "Projeto não encontrado");

        if ((int)project.ProjectStatus > (int)request.ProjectStatus)
            return new Response<string>(null, 400, "Não é possível retroceder o status do projeto");

        project.ProjectStatus = request.ProjectStatus;
        project.UserId = request.UserId;
        project.UpdatedAt = DateTime.Now.ToUnspecifiedKind();

        switch (project.ProjectStatus)
        {
            case EProjectStatus.Development:
                project.DevelopmentStart = DateTime.Now.ToUnspecifiedKind();
                break;
            case EProjectStatus.Validation:
                project.ValidationStart = DateTime.Now.ToUnspecifiedKind();
                project.DevelopmentEnd = DateTime.Now.ToUnspecifiedKind();
                break;
            case EProjectStatus.Finished:
                project.FinishedIn = DateTime.Now.ToUnspecifiedKind();
                project.DevelopmentEnd = DateTime.Now.ToUnspecifiedKind();
                project.ValidationEnd = DateTime.Now.ToUnspecifiedKind();
                break;
        }

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Status do projeto {ProjectId} atualizado para {ProjectStatus} pelo usuário {UserName}", 
            project.Id, project.ProjectStatus, request.UserId);

        return new Response<string>(null, 204);
    }

    public async Task<Response<string>> UpdatePaymentStatusAsync(UpdatePaymentStatusProjectRequest request)
    {
        var project = await _context
            .Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive);

        if (project is null)
            return new Response<string>(null, 404, "Projeto não encontrado");


        if ((int)project.PaymentStatus > (int)request.PaymentStatus)
            return new Response<string>(null, 400, "Não é possível retroceder o status do pagamento");

        project.PaymentStatus = request.PaymentStatus;
        project.UserId = request.UserId;
        project.UpdatedAt = DateTime.Now.ToUnspecifiedKind();

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Status de pagamento do projeto {ProjectId} atualizado para {PaymentStatus} pelo usuário {UserName}",
            project.Id, project.PaymentStatus, request.UserId);

        return new Response<string>(null, 204);
    }

    public async Task<Response<ProjectCommissionResponse>> GetCommissionsAsync(GetProjectByIdRequest request)
    {
        var project = await _context
            .Projects
            .Include(p => p.Developers)
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive);

        if (project is null)
            return new Response<ProjectCommissionResponse>(null, 404, "Projeto não encontrado");

        var allDevelopers = await _context
            .Developers
            .Where(d => d.IsActive)
            .ToListAsync();

        var commissionService = new CommissionService(project, allDevelopers);
        var response = commissionService.CalculateCommision();
        return new Response<ProjectCommissionResponse>(response.Data);
    }

    public async Task<Response<ProjectBudgetResponse>> GetBudgetByProjectAsync(GetBudgetByProjectRequest request)
    {
        try
        {
            var resultProject = await GetByIdAsync(new GetProjectByIdRequest { Id = request.Id });
            if (!resultProject.IsSuccess)
                return new Response<ProjectBudgetResponse>(null, resultProject.Code, resultProject.Message);
            var project = resultProject.Data!;

            var resultSettings = await _appSettingsHandler.GetAppSettingsAsync();
            if (!resultSettings.IsSuccess)
                return new Response<ProjectBudgetResponse>(null, resultSettings.Code, resultSettings.Message);
            var settings = resultSettings.Data!;

            var filePath = Path.Combine(Configuration.Budgets.Path, "Budget.xlsx");

            using var package = new ExcelPackage(new FileInfo(filePath));

            var worksheet = package.Workbook.Worksheets["Orçamento"];

            var idString = project.Id.ToString().PadLeft(4, '0');
            worksheet.Cells["A7"].Value = $"Nº {idString}";
            worksheet.Cells["A8"].Value = project.Title;

            var tableServices = worksheet.Tables["Tabela7"];
            var startRow = tableServices.Address.Start.Row + 1;
            var startColumn = tableServices.Address.Start.Column;
            var endColumn = tableServices.Address.End.Column;
            var lastTableRow = tableServices.Address.End.Row;
            var rows = tableServices.Address.End.Row - tableServices.Address.Start.Row;
            var styleName = tableServices.StyleName;
            var tableStyle = tableServices.TableStyle;

            worksheet.Tables.Delete(tableServices);

            var currentRow = lastTableRow;
            var rowsToAdd = project.ProjectServices.Count == 1
                ? 1
                : project.ProjectServices.Count - rows;
            if (rowsToAdd > 1)
                worksheet.InsertRow(currentRow, rowsToAdd);
            const int fontSize = 10;
            var indexService = 1;
            foreach (var service in project.ProjectServices)
            {
                worksheet.Cells[currentRow, startColumn].Value = indexService.ToString().PadLeft(2, '0');
                worksheet.Cells[currentRow, startColumn].Style.Font.Size = fontSize;
                worksheet.Cells[currentRow, startColumn + 1].Value = service.Description;
                worksheet.Cells[currentRow, startColumn + 1].Style.Font.Size = fontSize;
                worksheet.Cells[currentRow, startColumn + 2].Value = service.Hours;
                worksheet.Cells[currentRow, startColumn + 2].Style.Font.Size = fontSize;
                worksheet.Cells[currentRow, startColumn + 3].Value = decimal.ToInt32(service.Value);
                worksheet.Cells[currentRow, startColumn + 3].Style.Font.Size = fontSize;
                worksheet.Cells[currentRow, startColumn + 3].Style.Numberformat.Format = "#,##0.00";
                currentRow++;
                indexService++;
            }

            var endRow = currentRow - 1;
            var newTableRange = worksheet.Cells[startRow - 1, startColumn, endRow, endColumn];

            worksheet.Cells[$"D{currentRow + 1}"].Value = project.ProjectServices.Sum(p => p.Hours);
            worksheet.Cells[$"D{currentRow + 2}"].Value = settings.ValuePerHour;
            worksheet.Cells[$"D{currentRow + 5}"].Value = project.ProjectServices.Sum(p => p.Value);
            worksheet.Cells[$"D{currentRow + 8}"].Value = $"{project.Deadline} Dias úteis";

            var newTable = worksheet.Tables.Add(newTableRange, "Tabela7");
            newTable.TableStyle = tableStyle;
            newTable.StyleName = styleName;

            var bytes = await package.GetAsByteArrayAsync();

            var fileContent = new ProjectBudgetResponse
            {
                FileContents = bytes,
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileDownloadName = $"Orçamento-{idString}.xlsx"
            };
            return new Response<ProjectBudgetResponse>(fileContent);
        }
        catch (Exception e)
        {
            return new Response<ProjectBudgetResponse>(null, 500, e.Message);
        }
    }
}