namespace Bot.Database.Entities {
  public class StateInteger {
    public StateInteger(string key, long value) {
      Key = key;
      Value = value;
    }

    public StateInteger() {

    }

    public string Key { get; set; }
    public long Value { get; set; }

  }
}
