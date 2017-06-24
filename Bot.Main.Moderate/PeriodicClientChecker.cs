using System;
using System.Threading.Tasks;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Main.Moderate {
  public class PeriodicClientChecker : ICommandHandler {
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;
    private readonly ILogger _logger;
    private readonly IClient _client;

    public PeriodicClientChecker(
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      ITimeService timeService,
      ISettings settings,
      ILogger logger,
      IClient client
    ) {
      _periodicTaskFactory = periodicTaskFactory;
      _timeService = timeService;
      _settings = settings;
      _logger = logger;
      _client = client;
    }

    public void Handle() {
      _periodicTaskFactory.Create(_settings.ClientCheckerInterval, () => {
        if (_timeService.UtcNow - _client.LatestReceivedAt > _settings.ClientCheckerInterval) {
          _logger.LogWarning($"Client's {nameof(_client.LatestReceivedAt)}/Now difference exceeds the {_settings.ClientCheckerInterval.ToPretty(_logger)} limit. Disconnecting...");
          _client.Disconnect();
        }
      });
    }

  }
}
