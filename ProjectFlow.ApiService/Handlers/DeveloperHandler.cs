using Microsoft.EntityFrameworkCore;
using ProjectFlow.ApiService.Data;
using ProjectFlow.Core.Extension;
using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Developers.Request;
using ProjectFlow.Core.Models.DTOs.Developers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Handlers;

public class DeveloperHandler : IDeveloperHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeveloperHandler> _logger;

    public DeveloperHandler(AppDbContext context, 
        ILogger<DeveloperHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Response<DeveloperResponse>> CreateAsync(DeveloperRequest request)
    {
        var user = await _context.Users.FindAsync(request.User.Id);
        if (user is null)
            return new Response<DeveloperResponse>(null, 404, "Usuário não encontrado");

        var developer = request.ToDeveloper();

        await _context.Developers.AddAsync(developer);
        await _context.SaveChangesAsync();

        var response = developer.ToDeveloperResponse();

        _logger.LogInformation("Desenvolvedor {Name} pelo usuário {UserName}", developer.Name, user.GivenName);

        return new Response<DeveloperResponse>(response, 201);
    }

    public async Task<Response<DeveloperResponse>> UpdateAsync(DeveloperRequest request)
    {
        var user = await _context.Users.FindAsync(request.User.Id);
        if (user is null)
            return new Response<DeveloperResponse>(null, 404, "Usuário não encontrado");

        var developer = await _context
            .Developers
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive);

        if (developer is null)
            return new Response<DeveloperResponse>(null, 404, "Desenvolvedor não encontrado");

        developer.Name = request.Name;
        developer.DevLevel = request.DevLevel;
        developer.UpdatedAt = DateTime.Now.ToUnspecifiedKind();

        _context.Developers.Update(developer);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Desenvolvedor {Name} atualizado pelo usuário {UserName}", developer.Name, user.GivenName);

        var response = developer.ToDeveloperResponse();

        return new Response<DeveloperResponse>(response);
    }

    public async Task<Response<DeveloperResponse>> DeleteAsync(DeleteDeveloperRequest request)
    {
        var user = await _context.Users.FindAsync(request.User.Id);
        if (user is null)
            return new Response<DeveloperResponse>(null, 404, "Usuário não encontrado");

        var developer = await _context
            .Developers
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive);

        if (developer is null)
            return new Response<DeveloperResponse>(null, 404, "Desenvolvedor não encontrado");

        developer.IsActive = false;
        developer.UpdatedAt = DateTime.Now.ToUnspecifiedKind();

        _context.Developers.Update(developer);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Desenvolvedor {Name} excluído pelo usuário {UserName}", developer.Name, user.GivenName);

        return new Response<DeveloperResponse>(null, 204);
    }

    public async Task<Response<DeveloperResponse>> GetByIdAsync(GetDeveloperByIdRequest request)
    {
        var developer = await _context
            .Developers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive);

        var response = developer?.ToDeveloperResponse();

        return response is null
            ? new Response<DeveloperResponse>(null, 404, "Desenvolvedor não encontrado")
            : new Response<DeveloperResponse>(response);
    }

    public async Task<PagedResponse<List<DeveloperResponse>>> GetAllAsync(GetAllDevelopersRequest request)
    {
        var query = _context
            .Developers
            .AsNoTracking()
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name);

        var developers = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var lowSearchTerm = request.SearchTerm.ToLower();
            developers = developers.Where(d =>
                d.Id.ToString().Contains(lowSearchTerm) ||
                d.Name.ToLower().Contains(lowSearchTerm) ||
                d.DevLevel.GetDisplayName().ToLower().Contains(lowSearchTerm)
            ).ToList();
        }

        var count = developers.Count;

        var response = developers.Select(d => d.ToDeveloperResponse()).ToList();

        return new PagedResponse<List<DeveloperResponse>>(response, count, request.PageNumber, request.PageSize);
    }
}