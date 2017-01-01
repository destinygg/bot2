namespace Bot.Database.Contracts {
  public interface IApiFactory {
    IStateVariablesApi GetStateVariablesApi { get; }
  }
}
