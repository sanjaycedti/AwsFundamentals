using System;
using MediatR;
using Customers.Consumer.Messages;

namespace Customers.Consumer.Handlers;

public class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
{
    private readonly ILogger<CustomerCreatedHandler> _logger;

    public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerCreated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Customer created: {request.FullName}");

        // throw new Exception($"Customer failed: {request.FullName}");

        return Task.CompletedTask;
    }
}

