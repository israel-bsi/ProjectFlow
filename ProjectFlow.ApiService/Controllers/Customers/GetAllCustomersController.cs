using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Customers.Request;
using ProjectFlow.Core.Models.DTOs.Customers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Customers;

[ApiController]
[Tags("Customer")]
public class GetAllCustomersController : ControllerBase
{
    private readonly ICustomerHandler _handler;

    public GetAllCustomersController(ICustomerHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/customers")]
    [EndpointSummary("Get all customers")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResponse<List<CustomerResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetAllCustomers(
        [FromQuery] int pageNumber = Core.Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Core.Configuration.DefaultPageSize,
        [FromQuery] string searchTerm = "")
    {
        var request = new GetAllCustomersRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm
        };
        var result = await _handler.GetAllAsync(request);

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}