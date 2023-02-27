using System;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messages;
using MediatR;
using Microsoft.Extensions.Options;

namespace Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IOptions<QueueSettings> _queueSettings;
    private readonly IAmazonSQS _sqs;
    private readonly IMediator _mediator;
    private readonly ILogger<QueueConsumerService> _logger;

	public QueueConsumerService(IOptions<QueueSettings> queueSettings, IAmazonSQS sqs, IMediator mediator, ILogger<QueueConsumerService> logger)
	{
        _queueSettings = queueSettings;
        _sqs = sqs;
        _mediator = mediator;
        _logger = logger;
	}

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name);

        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            AttributeNames = new List<string> { "All" },
            MessageAttributeNames = new List<string> { "All" },
            MaxNumberOfMessages = 1
        };

        while(!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqs.ReceiveMessageAsync(receiveMessageRequest,stoppingToken);

            foreach(var message in response.Messages)
            {
                var messageType = message.MessageAttributes["MessageType"].StringValue;
                var type = Type.GetType($"Customers.Consumer.Messages.{messageType}");

                if (type is null)
                {
                    _logger.LogWarning("Unknown message type: {MessageType}", messageType);

                    continue;
                }

                var typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type)!;

                try
                {
                    await _mediator.Send(typedMessage, stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Message failed during processing");

                    continue;
                }


                await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl,  message.ReceiptHandle, stoppingToken);
            }


            await Task.Delay(1000, stoppingToken);
        }

    }
}

