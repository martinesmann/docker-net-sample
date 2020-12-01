using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _apiClient;
        private readonly HttpClient _daprClient;


        public string Message { get; private set; } = "PageModel in C#";

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _apiClient = clientFactory.CreateClient("api");
            _daprClient = clientFactory.CreateClient("dapr");
        }

        public async Task OnGet()
        {
            _logger.LogInformation("Index called");
            await GetApiData();
            await SendNotificationToApi();
        }

        private async Task GetApiData()
        {
            _logger.LogInformation("GetApiData called");

            try
            {
                var response = await _apiClient.GetAsync("/Random");

                response.EnsureSuccessStatusCode();
                Message = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Api data loaded ({Message})");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.LogError(ex, "get api data error");
            }
        }

        private async Task SendNotificationToApi()
        {
            _logger.LogInformation("SendNotificationToApi called");
            string PUBSUB_NAME = "messagebus";
            string TOPIC_NAME = "notification_topic";

            try
            {
                var payload = JsonSerializer.Serialize(new { name = "notification", content = "some random value (" + Guid.NewGuid().ToString("D") + ")" });

                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await _daprClient.PostAsync($"v1.0/publish/{PUBSUB_NAME}/{TOPIC_NAME}", content);

                _logger.LogInformation($"Notification send ({response.IsSuccessStatusCode}) - ({payload})");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Notification error ({response.IsSuccessStatusCode}) - ({await response.Content.ReadAsStringAsync()})");
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                _logger.LogError(ex, "Notification send error");
            }
        }
    }
}
