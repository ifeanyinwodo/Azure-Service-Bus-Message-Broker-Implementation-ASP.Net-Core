using Azure.Messaging.ServiceBus;
using ItemModel_Nugget;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Producer.IOC
{
    public class AzureBusService : IAzureBusService
    {
        private readonly IConfiguration _config;

        public AzureBusService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync(Item message)
        {
            await using (ServiceBusClient client = new ServiceBusClient(_config.GetConnectionString("AzureServiceBus")))
            {
                var queue = _config.GetSection("AzureServiceBusQueue").GetSection("Queue").Value;
                ServiceBusSender sender = client.CreateSender(queue);
                var messageStr = JsonConvert.SerializeObject(message);
                ServiceBusMessage serviceBusMessage = new ServiceBusMessage(messageStr);
                await sender.SendMessageAsync(serviceBusMessage);
            }
            
        }
    }
}
