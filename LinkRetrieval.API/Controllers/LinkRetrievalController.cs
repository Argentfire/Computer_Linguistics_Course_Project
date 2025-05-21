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

    public LinkRetrievalController(IHttpClientFactory clientFactory, ISearchService searchService, ApplicationDbContext db)
    {
      _clientFactory = clientFactory;
      _searchService = searchService;
      _db = db;
    }

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
      catch (Exception)
      {

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
