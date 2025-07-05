using Microsoft.EntityFrameworkCore;
using ProjectFlow.ApiService.Data;
using ProjectFlow.Core.Extension.Mapper;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Customers.Request;
using ProjectFlow.Core.Models.DTOs.Customers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Handlers;

public class CustomerHandler : ICustomerHandler
{
    private readonly AppDbContext _context;

    public CustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<CustomerResponse>> GetByIdAsync(GetCustomerByIdRequest request)
    {
        var customer = await _context
            .Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive);

        var response = customer?.ToCustomerResponse();

        return response is null
            ? new Response<CustomerResponse>(null, 404, "Cliente não encontrado")
            : new Response<CustomerResponse>(response);
    }

    public async Task<PagedResponse<List<CustomerResponse>>> GetAllAsync(GetAllCustomersRequest request)
    {
        var query = _context
            .Customers
            .AsNoTracking()
            .Where(c => c.IsActive);

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var lowSearchTerm = request.SearchTerm.ToLower();
            query = query.Where(c =>
                c.Id.ToString().Contains(lowSearchTerm) ||
                c.Name.ToLower().Contains(lowSearchTerm) ||
                c.DocumentNumber.Contains(lowSearchTerm)
            );
        }

        query = query.OrderBy(c => c.Name);

        var customers = await query
            .Skip((request.PageNumber - 1) * request.PageSize) // 1-1 = 0 -> 0 * 25 = 0 depois 2-1 = 1 -> 1 * 25 = 25
            .Take(request.PageSize) //25 itens
            .ToListAsync();

        var count = await query.CountAsync();

        var response = customers.Select(c => c.ToCustomerResponse()).ToList();

        return new PagedResponse<List<CustomerResponse>>(response, count, request.PageNumber, request.PageSize);
    }
}