namespace Bot.Database.Contracts {
  public interface IApiFactory {
    IStateIntegerApi GetStateIntegerApi { get; }
  }
}
