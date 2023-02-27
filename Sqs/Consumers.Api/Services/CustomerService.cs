using System;
using Consumers.Api.Contracts.Messages;
using Consumers.Api.Domain;
using Consumers.Api.Mapping;
using Consumers.Api.Messaging;
using Consumers.Api.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace Consumers.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService _gitHubService;
    private readonly ISqsMessenger _sqsMessenger;

    public CustomerService(ICustomerRepository customerRepository, IGitHubService gitHubService, ISqsMessenger sqsMessenger)
	{
        _customerRepository = customerRepository;
        _gitHubService = gitHubService;
        _sqsMessenger = sqsMessenger;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var existingCustomer = await _customerRepository.GetAsync(customer.Id);

        if (existingCustomer is not null)
        {
            var message = $"A customer with Id {customer.Id} already exists";

            throw new ValidationException(message, GenerateValidationError(nameof(Customer), message));
        }

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);

        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        var customerDto = customer.ToCustomerDto();

        var response = await _customerRepository.CreateAsync(customerDto);

        if (response)
        {
            await _sqsMessenger.SendMessageAsync(customer.ToCustomerCreatedMessage());
        }

        return response;
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

    public async Task<bool> UpdateAsync(Customer customer)
    {
        var customerDto = customer.ToCustomerDto();

        var isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);

        if (!isValidGitHubUser)
        {
            var message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        var response = await _customerRepository.UpdateAsync(customerDto);

        if (response)
        {
            await _sqsMessenger.SendMessageAsync(customer.ToCustomerUpdatedMessage());
        }

        return response;

    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _customerRepository.DeleteAsync(id);

        if (response)
        {
            await _sqsMessenger.SendMessageAsync(new CustomerDeleted { Id = id });
        }

        return response;
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new[]
        {
            new ValidationFailure(paramName, message)
        };
    }
}

