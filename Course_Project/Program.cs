using System.Text.RegularExpressions;


namespace Course_Project
{
  class Program
  {
    public static HttpClient? _client;
    public static string URLAddress { get; set; }
    public static string Content { get; set; }
    public static string RegExPattern { get; set; }

    static void Main(string[] args)
    {
      InitComponents();

      Console.WriteLine("Please enter a URL to begin scanning(copy URL and click right mouse button in terminal to paste it):");
      var url = Console.ReadLine();
      if (!string.IsNullOrEmpty(url))
      {
        URLAddress = url;

        var result = _client.GetAsync(URLAddress).Result;
        if (result.IsSuccessStatusCode)
        {
          Console.WriteLine("URL is valid. Scanning...");
          var content = result.Content.ReadAsStringAsync().Result;
          Content = content;

          Console.WriteLine("Please enter a regular expression pattern to search in the site:");
          var pattern = Console.ReadLine();
          if (!string.IsNullOrEmpty(pattern))
          {
            RegExPattern = pattern;
          }
          else
          {
            Console.WriteLine("Pattern is empty. Exiting...");
            return;
          }

          var links = GetLinks(content, RegExPattern);
          Console.WriteLine($"Found {links.Count} links on the page.");
          foreach (var link in links)
          {
            Console.WriteLine(link);
          }
        }
        else
        {
          Console.WriteLine("URL is invalid. Exiting...");
        }
      }
      else
      {
        Console.WriteLine("URL is empty. Exiting...");
        return;
      }
    }

    private static void InitComponents()
    {
      _client = new HttpClient();
    }

    private static List<string> GetLinks(string content, string pattern)
    {
      var links = new List<string>();
      var regex = new Regex(pattern, RegexOptions.IgnoreCase);
      var matches = regex.Matches(content);
      foreach (Match match in matches)
      {
        var link = match.Groups[2].Value;
        var finalLink = string.Empty;
        var protocolValidator = new Regex(@"https?://\S+", RegexOptions.IgnoreCase);
        if (!protocolValidator.IsMatch(link))
        {
          finalLink = $"{URLAddress}{link}";
        }
        else
        {
          finalLink = link;
        }
        links.Add(finalLink);
      }
      return links;
    }
  }
}
