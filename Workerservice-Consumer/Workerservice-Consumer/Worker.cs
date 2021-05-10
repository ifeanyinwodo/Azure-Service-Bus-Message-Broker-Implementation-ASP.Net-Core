using Azure.Messaging.ServiceBus;
using ItemModel_Nugget;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Workerservice_Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            
           
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
           
            Item item = JsonConvert.DeserializeObject<Item>(args.Message.Body.ToString());

            Console.WriteLine("Product Name " + item.Name);
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await using (ServiceBusClient client = new ServiceBusClient(_config.GetConnectionString("AzureServiceBus")))
                {
                    var queue = _config.GetSection("AzureServiceBusQueue").GetSection("Queue").Value;
                    ServiceBusProcessor processor = client.CreateProcessor(queue, new ServiceBusProcessorOptions());
                    processor.ProcessMessageAsync += MessageHandler;
                    processor.ProcessErrorAsync += ErrorHandler;
                    await processor.StartProcessingAsync();
                    Thread.Sleep(1000);
                    await processor.StopProcessingAsync();

                    
                }
            }
        }
    }
}
