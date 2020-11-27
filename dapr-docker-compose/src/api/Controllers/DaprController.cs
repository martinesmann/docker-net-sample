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
        }

        [HttpGet("subscribe")]
        public ActionResult Subscribe()
        {
            var payload = new[]
            {
                new {pubsubname="messagebus", topic= "notification", route = "notification" }
            };
            return Ok(payload);
        }

        [HttpPost("/notification")]
        public ActionResult Notification(CloudEvent cloudEvent)
        {
            _logger.LogDebug($"Cloud event {cloudEvent.Id} {cloudEvent.Type} {cloudEvent.DataContentType}");
            _logger.LogInformation("Notification received...");
            return Ok();
        }
    }
}
