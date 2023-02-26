using System;
using Consumers.Api.Domain;
using Consumers.Api.Mapping;
using Consumers.Api.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace Consumers.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
	{
        _customerRepository = customerRepository;

    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var existingCustomer = await _customerRepository.GetAsync(customer.Id);

        if (existingCustomer is not null)
        {
            var message = $"A customer with Id {customer.Id} already exists";

            throw new ValidationException(message, GenerateValidationError(nameof(Customer), message));
        }

        var customerDto = customer.ToCustomerDto();

        return await _customerRepository.CreateAsync(customerDto);
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        var customerDto = await _customerRepository.GetAsync(id);

        return customerDto?.ToCustomer();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var customerDtos = await _customerRepository.GetAllAsync();

        return customerDtos.Select(c => c.ToCustomer());
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Customer customer)
    {
        throw new NotImplementedException();
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new[]
        {
            new ValidationFailure(paramName, message)
        };
    }
}

