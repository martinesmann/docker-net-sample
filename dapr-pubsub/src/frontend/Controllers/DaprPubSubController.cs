using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frontend.Models;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace frontend.Controllers
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

        [Topic("pubsub", "newWeather")]
        [HttpPost("/weather")]
        public async Task<ActionResult> CreateWeather(Weather weather)
        {
            await Task.Delay(1);
            _logger.LogInformation("weather {weather}", weather);
            Program.Cache.Add(weather.Json);
            return Ok();
        }

        [Topic("pubsub", "newAssetReady")]
        [HttpPost("/asset")]
        public async Task<ActionResult> AssetReady(AssetCreatedModel model)
        {
            await Task.Delay(1);
            var etag = model.AssetEtag;

            _logger.LogInformation("======> Asset Ready {etag}", etag);

            Program.AssetCache.Add(model);

            return Ok();
        }


    }
}
