using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace frontend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssetsController : ControllerBase
    {

        private readonly ILogger<AssetsController> _logger;

        public AssetsController(ILogger<AssetsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Get called");

            return Ok();
        }

        [HttpPost]
        async public Task<IActionResult> Post([FromForm] FileModel model)
        {
            _logger.LogInformation("=====> POST | Asset Controller");
            _logger.LogInformation($"=====> filename: {model?.FileName ?? "no file"}");
            _logger.LogInformation($"=====> length: {model?.FormFile?.Length ?? 0}");

            // send file to Assets Service using dapr pub/sub:
            byte[] buffer = null;
            using (var memoryStream = new MemoryStream())
            {
                await model.FormFile.CopyToAsync(memoryStream);
                memoryStream.ToArray();
                buffer = memoryStream.GetBuffer();
            }


            var data = new AssetData
            {
                FileName = model.FileName,
                Data = buffer
            };

            var daprClient = new DaprClientBuilder().Build();
            await daprClient.PublishEventAsync<AssetData>("pubsub", "newAsset", data);


            // wait for asset server to post response
            AssetCreatedModel assetCreateModel;
            while (!Program.AssetCache.TryTake(out assetCreateModel, (int)TimeSpan.FromSeconds(5).TotalMilliseconds))
            {
                // please note that using Program.Cache is a hack (Should be replaced with SignalR)
                await Task.Delay(1000);
            }

            _logger.LogInformation($"========> Asset service published data to pubsub {assetCreateModel}", assetCreateModel);
            _logger.LogInformation($"========> new asset created FileName: {assetCreateModel.FileName} | {assetCreateModel.AssetTemplateName}");


            return Ok();
        }
    }
}
