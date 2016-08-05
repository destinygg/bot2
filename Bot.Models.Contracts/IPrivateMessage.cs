namespace Bot.Models.Contracts {
  public interface IPrivateMessage : ISendable {
    string Text { get; }
  }
}
