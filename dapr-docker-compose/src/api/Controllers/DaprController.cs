using CloudNative.CloudEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using web.Datacontext;
using web.model;

namespace web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DaprController : ControllerBase
    {
        private readonly ILogger<DaprController> _logger;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public DaprController(ILogger<DaprController> logger, IDbContextFactory<ApplicationContext> contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
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
                    topic= "notification_topic",
                    route = "notification_post"
                }
            };

            return Ok(payload);
        }

        //[Topic("messagebus", "notification")]
        [HttpPost("/notification_post")]
        public ActionResult Notification(CloudEvent cloudEvent)
        {
            _logger.LogInformation("Notification received...");
            _logger.LogInformation($"Cloud event {cloudEvent.Id} {cloudEvent.Type} {cloudEvent.DataContentType} ({ cloudEvent.Data})");

            using (var context = _contextFactory.CreateDbContext())
            {
                if (context.Database.EnsureCreated())
                {
                    _logger.LogInformation($"Database created");
                }

                context.Messages.Add(
                    new MessageModel
                    {
                        Data = cloudEvent.Data.ToString(),
                        DataContentType = cloudEvent.DataContentType.Name,
                        CloudEventId = cloudEvent.Id,
                        CloudEventType = cloudEvent.Type
                    });

                context.SaveChanges();
            }

            return Ok();
        }
    }
}