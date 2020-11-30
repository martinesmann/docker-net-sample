using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DaprController : ControllerBase
    {
        private readonly ILogger<DaprController> _logger;

        public DaprController(ILogger<DaprController> logger)
        {
            _logger = logger;
            _logger.LogInformation("DaprController initialized...");
        }

        [HttpGet("subscribe")]
        public ActionResult Subscribe()
        {
            _logger.LogInformation("Subscribe called...");

            var payload = new[]
            {
                new {
                    pubsubname="messagebus",
                    topic= "notification",
                    route = "notification"
                }
            };

            return Ok(payload);
        }

        //[Topic("messagebus", "notification")]
        [HttpPost("/notification")]
        public ActionResult Notification(CloudEvent cloudEvent)
        {
            _logger.LogInformation("Notification received...");
            //_logger.LogInformation($"Cloud event {cloudEvent}");

            _logger.LogInformation($"Cloud event {cloudEvent.Id} {cloudEvent.Type} {cloudEvent.DataContentType}");
            return Ok();
        }
    }
}
