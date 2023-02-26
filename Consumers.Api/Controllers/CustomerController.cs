using System;
using Consumers.Api.Contracts.Requests;
using Consumers.Api.Mapping;
using Consumers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Consumers.Api.Controllers;

[ApiController]
public class CustomerController : ControllerBase
{
	private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
	{
		_customerService = customerService;
    }

	[HttpPost("customers")]
	public async Task<IActionResult> Create([FromBody] CustomerRequest request)
	{
		var customer = request.ToCustomer();

		await _customerService.CreateAsync(customer);

		var customerResponse = customer.ToCustomerResponse();

		return CreatedAtAction("Get", new { customerResponse.Id }, customerResponse);
	}

	[HttpGet("customers/{id:guid}")]
	public async Task<IActionResult> Get([FromRoute] Guid id)
	{
		var customer = await _customerService.GetAsync(id);

		if (customer is null)
		{
			return NotFound();
		}

		var customerResponse = customer.ToCustomerResponse();

		return Ok(customerResponse);
	}

	[HttpGet("customers")]
	public async Task<IActionResult> GetAll()
	{
		var customers = await _customerService.GetAllAsync();

		var customersResponse = customers.ToCustomersResponse();

		return Ok(customersResponse);
    }
}

