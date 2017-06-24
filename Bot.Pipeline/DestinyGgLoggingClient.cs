using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class DestinyGgLoggingClient : DestinyGgBaseClient {
    private readonly ILogger _logger;

    public DestinyGgLoggingClient(
      IPrivateConstants privateConstants,
      ILogger logger,
      ISettings settings,
      ITimeService timeService,
      IPipelineManager pipelineManager
    ) : base(privateConstants, logger, settings, timeService, pipelineManager) {
      _logger = logger;
    }

    public override void Send(string data) => _logger.LogInformation(data);

  }
}
