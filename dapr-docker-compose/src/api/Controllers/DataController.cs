using System.Linq;
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
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public DataController(ILogger<DataController> logger, IDbContextFactory<ApplicationContext> contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
            _logger.LogInformation("DataController initialized...");
        }

        //[Topic("messagebus", "notification")]
        [HttpGet()]
        public ActionResult Get()
        {
            _logger.LogInformation("Get called");

            using var context = _contextFactory.CreateDbContext();

            if (context.Database.EnsureCreated())
            {
                _logger.LogInformation($"Database created");
            }

            return Ok(context.Messages.ToList());
        }
    }
}