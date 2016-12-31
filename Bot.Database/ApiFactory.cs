using Bot.Pipeline.Contracts;

namespace Bot.Database {
  public class ApiFactory {
    private readonly ILogger _logger;

    public ApiFactory(ILogger logger) {
      _logger = logger;
    }

    public StateVariablesApi GetStateVariablesApi => new StateVariablesApi(_logger);
  }
}