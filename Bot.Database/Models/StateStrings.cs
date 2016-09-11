using SQLite;

namespace Bot.Database.Models
{
  public class StateStrings {
    [PrimaryKey, NotNull]
    public string Key { get; set; }
    public string Value { get; set; }
  }
}