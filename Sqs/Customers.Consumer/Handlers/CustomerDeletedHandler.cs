﻿using System;
using MediatR;
using Customers.Consumer.Messages;

namespace Customers.Consumer.Handlers
{
    public class CustomerDeletedHandler : IRequestHandler<CustomerDeleted>
    {
        private readonly ILogger<CustomerDeletedHandler> _logger;

        public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CustomerDeleted request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Customer deleted: {request.Id}");

            return Task.CompletedTask;
        }
    }
}

