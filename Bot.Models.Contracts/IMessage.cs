namespace Bot.Models.Contracts {
  public interface IMessage : ITransmittable {
    string Text { get; }
  }
}
