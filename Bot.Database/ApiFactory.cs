using Bot.Database.Contracts;
using Bot.Tools.Interfaces;

namespace Bot.Database {
  public class ApiFactory : IApiFactory {
    private readonly ILogger _logger;

    public ApiFactory(ILogger logger) {
      _logger = logger;
    }

    public IStateIntegerApi GetStateIntegerApi => new StateIntegerApi(_logger);
  }
}