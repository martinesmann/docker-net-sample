using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DaprPubSubController : ControllerBase
    {
        private readonly ILogger<DaprPubSubController> _logger;

        public DaprPubSubController(ILogger<DaprPubSubController> logger)
        {
            _logger = logger;
        }

        [Topic("pubsub", "newOrder")]
        [HttpPost("/orders")]
        public async Task<ActionResult> CreateOrder(Order order)
        {
            await Task.Delay(1);
            _logger.LogInformation("order {order}", order);
            return Ok();

        }


    }
}
