using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class DestinyGgLoggingClient : DestinyGgBaseClient {
    private readonly ILogger _logger;

    public DestinyGgLoggingClient(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService, IPipelineManager pipelineManager) : base(privateConstants, logger, timeService, pipelineManager) {
      _logger = logger;
    }

    public override void Send(string data) => _logger.LogInformation(data);

  }
}
