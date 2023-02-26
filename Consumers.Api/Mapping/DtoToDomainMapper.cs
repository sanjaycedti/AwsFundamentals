﻿using System;
using Consumers.Api.Contracts.Data;
using Consumers.Api.Domain;

namespace Consumers.Api.Mapping;

public static class DtoToDomainMapper
{
	public static Customer ToCustomer(this CustomerDto customerDto)
	{
		return new Customer
		{
			Id = customerDto.Id,
			GitHubUsername = customerDto.GitHubUsername,
			Email = customerDto.Email,
			FullName = customerDto.FullName,
			DateOfBirth = customerDto.DateOfBirth
		};
	}
}

