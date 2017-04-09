namespace Bot.Database.Entities {
  public class StateIntegerEntity {
    public StateIntegerEntity(string key, long value) {
      Key = key;
      Value = value;
    }

    public StateIntegerEntity() {

    }

    public string Key { get; set; }
    public long Value { get; set; }

  }
}
