using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;

namespace Microsoft.eShopWeb.ApplicationCore.Services;
public class MessageSenderService : IMessageSenderService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusSender _serviceBusSender;
    private readonly IConfiguration _configuration;


    public MessageSenderService(
        IAzureClientFactory<ServiceBusClient> serviceBusClientFactory,
        IConfiguration configuration)
    {
        _configuration = configuration;
        _serviceBusClient = serviceBusClientFactory.CreateClient(nameof(MessageSenderService));
        _serviceBusSender = _serviceBusClient.CreateSender(_configuration["OrdersQueueName"]);
    }

    public async Task SendOrderAsync(Order order)
    {
        ServiceBusMessage message = new(JsonSerializer.Serialize(order));
        await _serviceBusSender.SendMessageAsync(message);
    }
}
