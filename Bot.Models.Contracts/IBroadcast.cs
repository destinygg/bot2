namespace Bot.Models.Contracts {
  public interface IBroadcast : ISendable {
    string Text { get; }
  }
}
