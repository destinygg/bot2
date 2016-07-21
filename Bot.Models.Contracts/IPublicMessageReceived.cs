namespace Bot.Models.Contracts {
  public interface IPublicMessageReceived : IReceived {
    string Text { get; }
  }
}
