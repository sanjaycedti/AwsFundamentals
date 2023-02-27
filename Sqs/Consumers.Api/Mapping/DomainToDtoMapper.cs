﻿using System;
using Consumers.Api.Contracts.Data;
using Consumers.Api.Domain;

namespace Consumers.Api.Mapping;

public static class DomainToDtoMapper
{
	public static CustomerDto ToCustomerDto(this Customer customer)
	{
		return new CustomerDto
		{
			Id = customer.Id,
			Email = customer.Email,
			GitHubUsername = customer.GitHubUsername,
			FullName = customer.FullName,
			DateOfBirth = customer.DateOfBirth
		};
	}
}

