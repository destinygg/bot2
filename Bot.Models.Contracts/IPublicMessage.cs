namespace Bot.Models.Contracts {
  public interface IPublicMessage : ISendable {
    string Text { get; }
  }
}
