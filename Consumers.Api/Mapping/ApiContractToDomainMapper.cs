using System;
using Consumers.Api.Contracts.Requests;
using Consumers.Api.Domain;

namespace Consumers.Api.Mapping;

public static class ApiContractToDomainMapper
{
	public static Customer ToCustomer(this CustomerRequest request)
	{
		return new Customer
		{
			Id = Guid.NewGuid(),
			Email = request.Email,
			GitHubUsername = request.GitHubUsername,
			FullName = request.FullName,
			DateOfBirth = request.DateOfBirth
		};
	}
}

