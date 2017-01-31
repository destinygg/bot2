namespace Bot.Models.Interfaces {
  public interface IMessage : ITransmittable {
    string Text { get; }
  }
}
