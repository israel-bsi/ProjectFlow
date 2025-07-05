using Microsoft.AspNetCore.Mvc;
using ProjectFlow.ApiService.Common.Api;
using ProjectFlow.Core.Handlers;
using ProjectFlow.Core.Models.DTOs.Customers.Request;
using ProjectFlow.Core.Models.DTOs.Customers.Response;
using ProjectFlow.Core.Response;

namespace ProjectFlow.ApiService.Controllers.Customers;

[ApiController]
[Tags("Customer")]
public class GetCustomerByIdController : Controller
{
    private readonly ICustomerHandler _handler;

    public GetCustomerByIdController(ICustomerHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("v1/customers/{id:int}", Name = "GetCustomerById")]
    [EndpointSummary("Get customer by id")]
    [Authorize]
    [ProducesResponseType(typeof(Response<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetCustomerById(int id)
    {
        var request = new GetCustomerByIdRequest { Id = id };

        var result = await _handler.GetByIdAsync(request);

        return result.IsSuccess
            ? Ok(result)
            : this.ToActionResult(new ErrorData(result.Code, result.Message));
    }
}