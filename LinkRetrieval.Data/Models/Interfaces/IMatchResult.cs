using LinkRetrieval.Data.Models.Classes;

namespace LinkRetrieval.Data.Models.Interfaces
{
  public interface IMatchResult
  {
    public int Id { get; set; }

    // Текста на съвпадението
    public string MatchedText { get; set; }

    // Запазване на позицията в HTML-а (например стартов индекс)
    public int StartIndex { get; set; }

    // Дължината на съвпадащия текст
    public int Length { get; set; }

    // Външен ключ към записа на търсенето
    public Guid SearchId { get; set; }

    // Навигационно свойство за обратна връзка към записа (Search)
    public Search Search { get; set; }
  }
}
