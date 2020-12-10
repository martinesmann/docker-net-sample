using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _apiClient;
        private readonly HttpClient _daprClient;


        public string MessageApiData { get; private set; } = "ApiData";

        public string MessagePubSub { get; private set; } = "PubSub";

        public string MessageRedis { get; private set; } = "Redis";


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
            await RedisConnection();
        }

        private async Task GetApiData()
        {
            _logger.LogInformation("GetApiData called");

            try
            {
                var response = await _apiClient.GetAsync("/Random");

                response.EnsureSuccessStatusCode();
                MessageApiData = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Api data loaded ({MessageApiData})");
            }
            catch (Exception ex)
            {
                MessageApiData = ex.Message;
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
                MessagePubSub = $"Notification send ({response.IsSuccessStatusCode}) - ({payload})";
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Notification error ({response.IsSuccessStatusCode}) - ({await response.Content.ReadAsStringAsync()})");
                    MessagePubSub = $"Notification error ({response.IsSuccessStatusCode}) - ({await response.Content.ReadAsStringAsync()})";
                }
            }
            catch (Exception ex)
            {
                MessagePubSub = ex.Message;
                _logger.LogError(ex, "Notification send error");
            }
        }

        private async Task RedisConnection()
        {
            _logger.LogInformation("RedisConnection called");
            string REDIS_SERVER = "redis-server:6379";
            try
            {
                using ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect($"{REDIS_SERVER},password=S0m3P@$$w0rd");
                IDatabase conn = muxer.GetDatabase();
                var pong = await conn.PingAsync(CommandFlags.None);
                MessageRedis = $"Redis Ping -> ({pong}) to ({REDIS_SERVER})";

                _logger.LogInformation($"Redis Ping -> ({pong})");
            }
            catch (Exception ex)
            {
                MessageRedis = ex.Message;
                _logger.LogInformation($"Redis error ({ex.Message})");
                _logger.LogError(ex, $"Redis error ({ex.Message})");
            }
        }
    }
}
