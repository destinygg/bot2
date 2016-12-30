using SQLite;

namespace Bot.Database.Models {

  public class StateVariables {
    [PrimaryKey, NotNull]
    public string Key { get; set; }
    public long Value { get; set; }
  }
}