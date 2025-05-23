using LinkRetrieval.API.DTOs;
using LinkRetrieval.Core.Contracts;
using LinkRetrieval.Data.DB;
using LinkRetrieval.Data.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace LinkRetrieval.API.Controllers
{
  [ApiController]
  public class LinkRetrievalController : Controller
  {
    private readonly ISearchService _searchService;
    private readonly ApplicationDbContext _db;
    private readonly IHttpClientFactory _clientFactory;


    /// <summary>
    /// Constructor for LinkRetrievalController which initializes the IHttpClientFactory, ISearchService, and ApplicationDbContext private fields.
    /// </summary>
    /// <param name="clientFactory"></param>
    /// <param name="searchService"></param>
    /// <param name="db"></param>
    public LinkRetrievalController(IHttpClientFactory clientFactory, ISearchService searchService, ApplicationDbContext db)
    {
      _clientFactory = clientFactory;
      _searchService = searchService;
      _db = db;
    }

    /// <summary>
    /// HTTP GET method to retrieve all searches from the database.
    /// </summary>
    /// <returns>
    /// Returns a status code based on the result of the algorithm.
    /// Possible status codes:
    /// 200 - Success(OK method) containing the list of searches.
    /// 500 - Internal Server Error containing the caugh exception.
    /// </returns>
    [HttpGet]
    [Route("GetSearches")]
    public async Task<IActionResult> GetSearches()
    {
      try
      {
        var result = await _searchService.GetAllSearchesAsync();
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal Server Error: {ex.ToString()}");
      }
    }

    /// <summary>
    /// HTTP GET method to retrieve a specific search by its ID.
    /// </summary>
    /// <param name="id">ID of the item</param>
    /// <returns>Returns a status code based on the result of the algorithm.
    /// Possible status codes:
    /// 200 - Success(OK method) containing the search with the specified ID.
    /// 404 - Not Found if the an item with the specified ID does not exist.
    /// 500 - Internal Server Error containing the caugh exception.
    /// </returns>
    [HttpGet]
    [Route("GetSearch/{id}")]
    public async Task<IActionResult> GetSearch(Guid id)
    {
      try
      {
        var result = await _searchService.GetSearchWithIdAsync(id);
        if (result == null)
        {
          return NotFound($"Search with ID {id} not found.");
        }
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal Server Error: {ex.ToString()}");
      }
    }

    /// <summary>
    /// HTTP POST method to add a new search. This is the method that will be called when the user submits the search form.
    /// It performs the base functionality on which this project has been built - scraping a web page and searching for a regular expression.
    /// </summary>
    /// <param name="searchRequest">A DTO from "SearchReceiveNode" type containing the object on which the request is to be processed</param>
    /// <returns>Returns a status code based on the result of the algorithm.
    /// Possible staus codes:
    /// 200 - Success(OK method) containing a collection(List) of DTO from "MatchResultDto" type. The list contains the found matches for the specified regular expression on level 1 depth.
    /// 400 - Bad Request if the URL or regular expression is invalid or if an error occurs while retrieving the HTML content.
    /// 500 - Internal Server Error if an error occurs while executing the method.
    /// </returns>
    [HttpPost]
    [Route("AddSearch")]
    public async Task<IActionResult> AddSearch([FromBody] SearchReceiveNode searchRequest)
    {
      if (searchRequest == null || string.IsNullOrWhiteSpace(searchRequest.Url) || string.IsNullOrWhiteSpace(searchRequest.RegexPattern))
      {
        return BadRequest("Invalid search request. Please enter valid URL address and/or regular expression.");
      }

      var client = _clientFactory.CreateClient();
      string htmlContent;
      try
      {
        htmlContent = await client.GetStringAsync(searchRequest.Url);
      }
      catch (Exception ex)
      {
        return BadRequest($"Error retrieving HTML content: {ex.Message}");
      }

      Regex regex;
      try
      {
        regex = new Regex(searchRequest.RegexPattern);
      }
      catch (ArgumentException ex)
      {
        return BadRequest($"Invalid regular expression: {ex.Message}");
      }

      var matches = regex.Matches(htmlContent);
      var matchResults = new List<MatchResult>();
      foreach (Match match in matches)
      {
        matchResults.Add(new MatchResult
        {
          MatchedText = match.Value,
          StartIndex = match.Index,
          Length = match.Length
        });
      }

      var searchEntity = new Search
      {
        Url = searchRequest.Url,
        RegexPattern = searchRequest.RegexPattern,
        HtmlContent = htmlContent,
        SearchTime = DateTime.Now,
        MatchResults = matchResults
      };

      try
      {
        await _searchService.CreateSearchAsync(searchEntity);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal Server Error: {ex.Message}");
      }

      var matchResultDtos = new List<MatchResultDto>();
      foreach (var result in matchResults)
      {
        matchResultDtos.Add(new MatchResultDto
        {
          MatchedText = result.MatchedText,
          StartIndex = result.StartIndex,
          Length = result.Length
        });
      }

      if (searchRequest.SearchDepth > 1)
      {
        await DeepScanFindResults(searchEntity, searchRequest.RegexPattern, searchRequest.SearchDepth, 0);
      }

      return Ok(new { results = matchResultDtos });
    }

    /// <summary>
    /// A recursive method that performs a deep scan of the HTML content on specified depth to find matches for the specified regular expression.
    /// </summary>
    /// <param name="search">Reference to the created search object from the "AddSearch" HTTP GET method</param>
    /// <param name="regexPattern">Reference to the used regular expression from the initial search</param>
    /// <param name="scanDepth">Level of the depth at which the deep scan is to be performed</param>
    /// <param name="currentDepth">The level of depth at which the method is being called from</param>
    /// <returns>Returns a boolean value based on the successful execution of the method</returns>
    private async Task<bool> DeepScanFindResults(Search search, string regexPattern, int scanDepth, int currentDepth)
    {
      var client = _clientFactory.CreateClient();
      Regex regex = new Regex(regexPattern);
      for (var depthLevel = currentDepth; depthLevel < scanDepth; depthLevel++)
      {

        foreach (var item in search.MatchResults)
        {
          try
          {
            var searchRequest = item.Search;
            string htmlContent = await client.GetStringAsync(item.MatchedText);
            var matches = regex.Matches(htmlContent);
            var matchResults = new List<MatchResult>();
            foreach (Match match in matches)
            {
              matchResults.Add(new MatchResult
              {
                MatchedText = match.Value,
                StartIndex = match.Index,
                Length = match.Length
              });
            }

            var searchEntity = new Search
            {
              Url = item.MatchedText,
              RegexPattern = regexPattern,
              HtmlContent = htmlContent,
              SearchTime = DateTime.Now,
              MatchResults = matchResults
            };

            try
            {
              await _searchService.CreateSearchAsync(searchEntity);
            }
            catch (Exception)
            {

            }

            await DeepScanFindResults(searchEntity, regexPattern, scanDepth, depthLevel + 1);
          }
          catch (Exception)
          {
            return false;
          }
        }
      }
      return true;
    }
  }
}
