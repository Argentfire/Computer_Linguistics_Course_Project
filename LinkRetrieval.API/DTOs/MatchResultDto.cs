namespace LinkRetrieval.API.DTOs
{
  public class MatchResultDto
  {
    public string MatchedText { get; set; }
    public int StartIndex { get; set; }
    public int Length { get; set; }
  }
}
