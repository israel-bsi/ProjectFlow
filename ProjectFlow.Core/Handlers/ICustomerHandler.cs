using ProjectFlow.Core.Models.DTOs.Customers.Request;
using ProjectFlow.Core.Models.DTOs.Customers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.Core.Handlers;

public interface ICustomerHandler
{
    Task<Response<CustomerResponse>> GetByIdAsync(GetCustomerByIdRequest request);
    Task<PagedResponse<List<CustomerResponse>>> GetAllAsync(GetAllCustomersRequest request);
}