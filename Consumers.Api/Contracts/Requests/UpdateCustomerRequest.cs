using System;
using Microsoft.AspNetCore.Mvc;

namespace Consumers.Api.Contracts.Requests;

public class UpdateCustomerRequest
{
	[FromRoute(Name = "id")] public Guid Id { get; init; }
	[FromBody] public CustomerRequest Customer { get; init; } = default!;
}

