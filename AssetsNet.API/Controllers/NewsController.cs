using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.News;
using AssetsNet.API.Models.News;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class NewsController : BaseApiController
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<News>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<News>>> Get([FromQuery] string companyName, [FromQuery] string region = "US")
    {
        try
        {
            var news = await _newsService.GetNewsAsync(companyName, region);
            return Ok(news);
        } 
        catch (HttpRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}