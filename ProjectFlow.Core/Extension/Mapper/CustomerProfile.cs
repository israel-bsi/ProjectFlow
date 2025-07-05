using ProjectFlow.Core.Models.DTOs.Customers.Request;
using ProjectFlow.Core.Models.DTOs.Customers.Response;
using ProjectFlow.Core.Models.Entities;

namespace ProjectFlow.Core.Extension.Mapper;

public static class CustomerProfile
{
    public static CustomerResponse ToCustomerResponse(this Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            BusinessName = customer.BusinessName,
            DocumentNumber = customer.DocumentNumber,
            Phone = customer.Phone,
            CellPhone = customer.CellPhone,
            PersonType = customer.PersonType,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }

    public static CustomerRequest ToCustomerRequest(this CustomerResponse customer)
    {
        return new CustomerRequest
        {
            Id = customer.Id,
            Name = customer.Name,
            BusinessName = customer.BusinessName,
            DocumentNumber = customer.DocumentNumber,
            Phone = customer.Phone,
            CellPhone = customer.CellPhone,
            PersonType = customer.PersonType
        };
    }
}