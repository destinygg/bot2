namespace Bot.Models.Contracts {
  public interface ISendable {
    ISendable Send();

    string ConsolePrint { get; }
  }
}
