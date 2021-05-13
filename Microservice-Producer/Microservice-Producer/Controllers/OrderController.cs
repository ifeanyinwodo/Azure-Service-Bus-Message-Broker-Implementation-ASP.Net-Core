using App.Metrics;
using ItemModel_Nugget;
using Microservice_Producer.IOC;
using Microservice_Producer.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IAzureBusService _azureBusService;
        private readonly IMetrics _metrics;
        public OrderController(IAzureBusService azureBusService, IMetrics metrics)
        {
            _azureBusService = azureBusService;
            _metrics = metrics;
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            _metrics.Measure.Counter.Increment(MetricsRegistry._sentMessage);
            await _azureBusService.SendMessageAsync(item);
           
            return Ok();



        }

    }
}
