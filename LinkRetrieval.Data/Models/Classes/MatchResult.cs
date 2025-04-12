using LinkRetrieval.Data.Models.Interfaces;

namespace LinkRetrieval.Data.Models.Classes
{
  public class MatchResult : IMatchResult
  {
    public virtual int Id { get; set; }
    public virtual string MatchedText { get; set; }
    public virtual int StartIndex { get; set; }
    public virtual int Length { get; set; }
    public virtual Guid SearchId { get; set; }
    public virtual Search Search { get; set; }
  }
}
