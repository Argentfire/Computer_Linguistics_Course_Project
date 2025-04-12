using LinkRetrieval.Data.Models.Interfaces;

namespace LinkRetrieval.Data.Models.Classes
{
  public class Search : ISearch
  {
    public virtual Guid Id { get; set; }
    public virtual string Url { get; set; }
    public virtual string RegexPattern { get; set; }
    public virtual string HtmlContent { get; set; }
    public virtual DateTime SearchTime { get; set; }
    public virtual ICollection<MatchResult> MatchResults { get; set; }
  }
}
