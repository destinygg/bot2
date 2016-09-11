using SQLite;

namespace Bot.Database.Models {
  public class JsonUserHistory {
    [PrimaryKey, AutoIncrement, NotNull]
    public int Id { get; set; }
    [NotNull, Unique]
    public string Nick { get; set; }
    public string RawHistory { get; set; }
  }
}