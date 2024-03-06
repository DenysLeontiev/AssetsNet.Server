using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Models.News;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AssetsNet.API.Controllers;

public class NewsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://apidojo-yahoo-finance-v1.p.rapidapi.com/auto-complete?q=google&region=US"),
            Headers =
    {
        { "X-RapidAPI-Key", "e93439e715msh56fb70def110954p1f8153jsn4cdb8cbdbce2" },
        { "X-RapidAPI-Host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
    },
        };
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        RootObject result = JsonConvert.DeserializeObject<RootObject>(body);

        foreach (var n in result.News)
        {
            Console.WriteLine(n.Thumbnail);
        }
        return Ok(result);
    }
}