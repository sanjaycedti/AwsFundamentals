﻿using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    FullName = "Sanjay Sahani",
    Email = "sanjaycedti@gmail.com",
    DateOfBirth = new DateTime(1980, 9, 4)
};


var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
        
    }
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine(response);