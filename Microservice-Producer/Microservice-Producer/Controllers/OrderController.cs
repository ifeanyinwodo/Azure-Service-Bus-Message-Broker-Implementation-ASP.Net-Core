using ItemModel_Nugget;
using Microservice_Producer.IOC;
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
        public OrderController(IAzureBusService azureBusService)
        {
            _azureBusService = azureBusService;

        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            await _azureBusService.SendMessageAsync(item);
            return Ok();



        }

    }
}
