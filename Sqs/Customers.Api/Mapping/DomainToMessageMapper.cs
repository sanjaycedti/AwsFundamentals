using System;
using Customers.Api.Contracts.Messages;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class DomainToMessageMapper
{
	public static CustomerCreated ToCustomerCreatedMessage(this Customer customer)
	{
		return new CustomerCreated
		{
			Id = customer.Id,
			GitHubUsername = customer.GitHubUsername,
			Email = customer.Email,
			FullName = customer.FullName,
			DateOfBirth = customer.DateOfBirth
		};
	}

    public static CustomerUpdated ToCustomerUpdatedMessage(this Customer customer)
    {
        return new CustomerUpdated
        {
            Id = customer.Id,
            GitHubUsername = customer.GitHubUsername,
            Email = customer.Email,
            FullName = customer.FullName,
            DateOfBirth = customer.DateOfBirth
        };
    }
}

