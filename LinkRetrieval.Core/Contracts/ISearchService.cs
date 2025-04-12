using LinkRetrieval.Data.Models.Interfaces;

namespace LinkRetrieval.Core.Contracts
{
  public interface ISearchService
  {
    Task CreateSearchAsync(ISearch search);
    Task RemoveSearchAsync(Guid id);
    Task<List<ISearch>> GetAllSearchesAsync();
    Task<ISearch> GetSearchWithIdAsync(Guid id);
    Task<ISearch> UpdateSearch(Guid id, ISearch search);
    Task ClearAllSearches();
  }
}
