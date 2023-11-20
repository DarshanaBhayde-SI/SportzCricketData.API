using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json;

namespace SportzCricketData.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CricketController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CricketController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string apiUrl = "http://feeds.sportz.io/cricket/live/json/calendar_new.json";

            try
            {
                using (HttpClient client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip }))
                {
                    var response = await client.GetAsync(apiUrl);   

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        dynamic data = JsonSerializer.Deserialize<ExpandoObject>(content);
                        
                        return Ok(data);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                    //return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception: {ex.Message}");
            }
        }
    }
}
