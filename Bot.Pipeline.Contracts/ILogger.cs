namespace Bot.Pipeline.Contracts {
  public interface ILogger {
    void LogWarning(string warning);
    void LogError(string error);
    void LogInformation(string information);
  }
}
