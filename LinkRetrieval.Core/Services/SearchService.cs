using LinkRetrieval.Core.Contracts;
using LinkRetrieval.Data.DB;
using LinkRetrieval.Data.Models.Classes;
using LinkRetrieval.Data.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinkRetrieval.Core.Services
{
  public class SearchService : ISearchService
  {
    private readonly ApplicationDbContext _db;

    public SearchService(ApplicationDbContext db)
    {
      _db = db;
    }

    public virtual async Task<List<ISearch>> GetAllSearchesAsync() => await _db.Searches.Include(s => s.MatchResults).ToListAsync<ISearch>();

    public async Task<ISearch> GetSearchWithIdAsync(Guid id) => await _db.Searches.FirstOrDefaultAsync(s => s.Id == id);
    public async Task RemoveSearchAsync(Guid id)
    {
      var search = (Search)(await GetSearchWithIdAsync(id));
      if (search == null)
      {
        throw new Exception("Search not found");
      }
      _db.Searches.Remove(search);
      await _db.SaveChangesAsync();
    }
    public async Task ClearAllSearches()
    {
      _db.Searches.RemoveRange(_db.Searches);
      await _db.SaveChangesAsync();
    }

    public async Task CreateSearchAsync(ISearch search)
    {
      var newSearch = new Search
      {
        Id = Guid.NewGuid(),
        Url = search.Url,
        RegexPattern = search.RegexPattern,
        HtmlContent = search.HtmlContent,
        SearchTime = DateTime.UtcNow,
        MatchResults = search.MatchResults
      };
      _db.Searches.Add(newSearch);
      await _db.SaveChangesAsync();
    }

    public async Task<ISearch> UpdateSearch(Guid id, ISearch search)
    {
      var existingSearch = (Search)(await GetSearchWithIdAsync(id));
      if (existingSearch == null)
      {
        throw new Exception("Search not found");
      }
      existingSearch.Url = search.Url;
      existingSearch.RegexPattern = search.RegexPattern;
      existingSearch.HtmlContent = search.HtmlContent;
      existingSearch.SearchTime = DateTime.UtcNow;
      existingSearch.MatchResults = search.MatchResults;
      _db.Searches.Update(existingSearch);
      await _db.SaveChangesAsync();
      return existingSearch;
    }
  }
}
