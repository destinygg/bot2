using SQLite;

namespace Bot.Database.Models {
  public class Stalk {
    [PrimaryKey, AutoIncrement, NotNull]
    public int Id { get; set; }
    [NotNull]
    public string Nick { get; set; }
    [NotNull]
    public int Time { get; set; }
    [NotNull]
    public string Text { get; set; }
  }
}