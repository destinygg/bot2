namespace Bot.Tools.Logging {
  public interface ILogPersister {
    void Persist(string log);
  }
}
