using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.News;
using AssetsNet.API.Models.News;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AssetsNet.API.Controllers;

public class NewsController : BaseApiController
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public async Task<ActionResult<Models.News.News>> Get([FromQuery] string companyName, [FromQuery] string region)
    {
        var news = await _newsService.GetNewsAsync(companyName, region);

        return Ok(news);
    }
}