using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            //// Original 
            // var rng = new Random();
            // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            // {
            //     Date = DateTime.Now.AddDays(index),
            //     TemperatureC = rng.Next(-20, 55),
            //     Summary = Summaries[rng.Next(Summaries.Length)]
            // })
            // .ToArray();

            //// raw api call
            // var client = new HttpClient();
            // client.BaseAddress = new Uri("http://backend-service:5002");

            // var response = await client.GetAsync("/WeatherForecast");
            // var data = await response.Content.ReadAsStringAsync();
            // _logger.LogWarning(data);
            // var obj = JsonConvert.DeserializeObject<dynamic>(data);
            // return Ok(obj);

            //// dapr invocation 
            try
            {
                HttpClient client = new HttpClient();
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
        }
    }
}
