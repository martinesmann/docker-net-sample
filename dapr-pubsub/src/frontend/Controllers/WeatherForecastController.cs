using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapr.Client;
using frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace frontend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        async public Task<IActionResult> Get()
        {
            ///
            /// Original
            /// 

            /*
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            */


            ///
            /// Direct Api call
            /// 

            /*
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://backend-service:5002");

            var response = await client.GetAsync("/WeatherForecast");
            var data = await response.Content.ReadAsStringAsync();
            _logger.LogWarning(data);
            var obj = JsonConvert.DeserializeObject<dynamic>(data);
            return Ok(obj);
            */


            ///
            /// Dapr method invocation
            /// 

            /*
            try
            {
                var client = new HttpClient();
                string darpSitecar = "localhost";
                int daprPort = 3500;
                string applicationId = "backend-service";
                string methodName = "WeatherForecast";
                var response = await client.GetAsync($"http://{darpSitecar}:{daprPort}/v1.0/invoke/{applicationId}/method/{methodName}");
                var data = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<dynamic>(data);
                return Ok(obj);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "dapr invocation failed");
                return Ok(ex.Message);
            }
            */


            /// 
            /// Dapr pubsub
            /// 

            var data = new OrderData
            {
                OrderId = "123456",
                ProductId = "67890",
                Amount = 2
            };

            var daprClient = new DaprClientBuilder().Build();

            await daprClient.PublishEventAsync<OrderData>("pubsub", "newOrder", data);

            string weatherData;
            while (!Program.Cache.TryTake(out weatherData, (int)TimeSpan.FromSeconds(5).TotalMilliseconds))
            {
                // please note that using Program.Cache is a hack (Should be replaced with SignalR)
                await Task.Delay(1000);
            }

            _logger.LogInformation("========> WeatherForecat got data from pubsub {weatherData}", weatherData);

            var obj = JsonConvert.DeserializeObject<dynamic>(weatherData);
            return Ok(obj);
        }
    }
}
