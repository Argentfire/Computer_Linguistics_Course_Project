using LinkRetrieval.Data.Models.Classes;

namespace LinkRetrieval.Data.Models.Interfaces
{
  public interface ISearch
  {
    public Guid Id { get; set; }

    // URL адресът, който е въведен от потребителя
    public string Url { get; set; }

    // Регулярен израз, използван при търсенето
    public string RegexPattern { get; set; }

    // Зареденият HTML (може да бъде опционално, ако искаш да запазваш съдържанието)
    public string HtmlContent { get; set; }

    // Времето на извършване на търсенето
    public DateTime SearchTime { get; set; }

    // Колекция от резултати (съвпадения), нанесени към това търсене
    public ICollection<MatchResult> MatchResults { get; set; }
  }
}
