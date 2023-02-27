using System;
using Consumers.Api.Contracts.Responses;
using Consumers.Api.Domain;

namespace Consumers.Api.Mapping;

public static class DomainToApiContractMapper
{
	public static CustomerResponse ToCustomerResponse(this Customer customer)
	{
		return new CustomerResponse
		{
			Id = customer.Id,
			Email = customer.Email,
			GitHubUsername = customer.GitHubUsername,
			FullName = customer.FullName,
			DateOfBirth = customer.DateOfBirth
		};
	}

    public static GetAllCustomersResponse ToCustomersResponse(this IEnumerable<Customer> customers)
    {
        return new GetAllCustomersResponse
        {
            Customers = customers.Select(x => ToCustomerResponse(x))
        };
    }
}

