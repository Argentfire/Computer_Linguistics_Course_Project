namespace LinkRetrieval.API.DTOs
{
  public class SearchReceiveNode
  {
    public string Url { get; set; }
    public string RegexPattern { get; set; }
    public int SearchDepth { get; set; }
  }
}
