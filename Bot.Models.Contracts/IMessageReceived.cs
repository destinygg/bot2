namespace Bot.Models.Contracts {
  public interface IMessageReceived : IMessage, IReceived {
    bool FromMod { get; }
  }
}
