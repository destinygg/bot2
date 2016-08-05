namespace Bot.Models.Contracts {
  public interface ISubonly : ISendable {
    bool IsEnabled { get; }
  }
}
