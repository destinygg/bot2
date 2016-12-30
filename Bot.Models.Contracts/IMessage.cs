namespace Bot.Models.Contracts {
  public interface IMessage {
    string Text { get; }
    bool StartsWith(string phrase);
  }
}
