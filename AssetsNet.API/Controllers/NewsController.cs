using System.Collections;
using AssetsNet.API.Controllers.Common;
using AssetsNet.API.ExtensionMethods.ClaimsPrincipalExtensionMethods;
using AssetsNet.API.Interfaces.News;
using AssetsNet.API.Interfaces.Photo;
using AssetsNet.API.Interfaces.Reddit;
using AssetsNet.API.Interfaces.Twitter;
using AssetsNet.API.Models.News;
using AssetsNet.API.Models.Reddit;
using AssetsNet.API.Models.Twitter;
using AssetsNet.API.Models.Twitter.TwitterUsersMedia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAPI.Models;

namespace AssetsNet.API.Controllers;

public class NewsController : BaseApiController
{
    private readonly INewsService _newsService;
    private readonly IRedditService _redditService;
    private readonly ITwitterService _twitterService;

    public NewsController(INewsService newsService, IRedditService redditService, ITwitterService twitterService)
    {
        _newsService = newsService;
        _redditService = redditService;
        _twitterService = twitterService;
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

    [Authorize]
    [HttpGet("reddit/{subreddit}/{redditTimePosted}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RedditPost>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RedditPost>>> GetSubreddits([FromRoute] string subreddit, [FromRoute] int redditTimePosted)
    {
        try
        {
            var userId = User.GetCurrentUserId();
            var data = await _redditService.GetRedditPosts(subreddit, redditTimePosted, userId);

            return Ok(data);
        }
        catch (GptRequestsLimitExceededException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (HttpRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpGet("twitter/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TwitterPost>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TwitterPost>>> GetTweets([FromRoute] string query, [FromQuery] int? searchType = null)
    {
        try
        {
            var userId = User.GetCurrentUserId();
            var data = await _twitterService.GetTwitterPosts(query, userId, searchType);

            return Ok(data);
        }
        catch (GptRequestsLimitExceededException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("twitter/userMedia/{screenName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TwitterUserMediaPost>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TwitterUserMediaPost>>> GetUserMedia(string screenName = "Stocktwits")
    {
        try
        {
            var data = await _twitterService.GetUserMedia(screenName);

            return Ok(data);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("newsApi/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Article>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Article>> GetNewsApi(string query)
    {
        try
        {
            var data = await _newsService.GetNewsApiArticles(query);

            return Ok(data);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}