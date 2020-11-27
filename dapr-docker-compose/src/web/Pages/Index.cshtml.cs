using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string Message { get; private set; } = "PageModel in C#";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            _logger.LogInformation("Index called");

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("http://api:5000/Random");

                response.EnsureSuccessStatusCode();
                Message = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Api data loaded ({Message})");
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
                _logger.LogError(ex, "get api data error");
            }
        }
    }
}
