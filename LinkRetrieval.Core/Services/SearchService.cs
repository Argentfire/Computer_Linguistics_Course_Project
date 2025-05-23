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
    /// <summary>
    /// Retrieves all searches from the database.
    /// </summary>
    /// <returns>A list containing all search objects in the database.</returns>
    public virtual async Task<List<ISearch>> GetAllSearchesAsync() => await _db.Searches.Include(s => s.MatchResults).ToListAsync<ISearch>();

    /// <summary>
    /// Retrieves a specific search object by its ID.
    /// </summary>
    /// <param name="id">The ID of the search object</param>
    /// <returns>The search object with specified ID.</returns>
    public async Task<ISearch> GetSearchWithIdAsync(Guid id) => await _db.Searches.FirstOrDefaultAsync(s => s.Id == id);

    /// <summary>
    /// Removes a search from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the desired object which is to be removed from the database.</param>
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

    /// <summary>
    /// Removes all searches from the database.
    /// </summary>
    /// <returns></returns>
    public async Task ClearAllSearches()
    {
      _db.Searches.RemoveRange(_db.Searches);
      await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Creates a new search in the database.
    /// </summary>
    /// <param name="search">The object which is to be created in the database</param>
    /// <returns></returns>
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

    /// <summary>
    /// Updates an existing search in the database.
    /// </summary>
    /// <param name="id">ID of the object which is to be updated</param>
    /// <param name="search">Object containing desired property values</param>
    /// <returns>The updated object</returns>
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
