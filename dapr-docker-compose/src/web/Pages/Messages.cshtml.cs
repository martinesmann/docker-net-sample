using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using web.Model;

namespace web.Pages
{
    public class MessageModel : PageModel
    {
        private readonly ILogger<MessageModel> _logger;
        private readonly HttpClient _apiClient;

        public IEnumerable<Message> Messages { get; private set; }

        public MessageModel(ILogger<MessageModel> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _apiClient = clientFactory.CreateClient("api");
        }

        public async Task OnGet()
        {
            _logger.LogInformation("Messages called");

            var json = await _apiClient.GetStringAsync("/data");
            Messages = JsonConvert.DeserializeObject<List<Message>>(json);
        }
    }
}
