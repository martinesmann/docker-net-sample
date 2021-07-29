using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DaprPubSubController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

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
            var orderDataLength = order.ProductId.Length;
            _logger.LogInformation("========> order data length (chars): {orderDataLength}", orderDataLength);


            var rng = new Random();
            var randomWeatherData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            var data = new WeatherData
            {
                Json = JsonConvert.SerializeObject(randomWeatherData, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })
            };

            string jsonStr = data.Json;

            _logger.LogInformation("====> Sending weather data {jsonStr}", jsonStr);


            var daprClient = new DaprClientBuilder().Build();

            await daprClient.PublishEventAsync<WeatherData>("pubsub", "newWeather", data);

            return Ok();
        }

        [Topic("pubsub", "newAsset")]
        [HttpPost("/asset")]
        public async Task<ActionResult> CreateAsset(Asset asset)
        {
            await Task.Delay(1);

            var dataLength = asset.Data.Length;
            _logger.LogInformation("========> asset data length (bytes): {dataLength}", dataLength);


            // TODO:  validate, scale, upload to blob storage

            var data = new AssetData
            {
                AssetEtag = Guid.NewGuid().ToString(),
                FileName = asset.FileName,
                AssetTemplateName = $"{asset.FileName}_#width#_#height#.jpeg"
            };

            _logger.LogInformation("====> Sending asset data to frontend {data}", data);
            var daprClient = new DaprClientBuilder().Build();
            await daprClient.PublishEventAsync<AssetData>("pubsub", "newAssetReady", data);

            return Ok();
        }
    }
}
