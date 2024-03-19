using AssetsNet.API.Controllers.Common;
using AssetsNet.API.Interfaces.News;
using AssetsNet.API.Interfaces.Reddit;
using AssetsNet.API.Models.News;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;

public class NewsController : BaseApiController
{
    private readonly INewsService _newsService;
    private readonly IRedditService _redditService;

    public NewsController(INewsService newsService, IRedditService redditService)
    {
        _newsService = newsService;
        _redditService = redditService;
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

    [HttpGet("reddit/{subreddit}/{redditTimePosted}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> GetSubreddits([FromRoute] string subreddit, [FromRoute] int redditTimePosted)
    {
        try
        {
            var data = await _redditService.GetRedditPosts(subreddit, redditTimePosted);

            return Ok(data);
        }
        catch (HttpRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}