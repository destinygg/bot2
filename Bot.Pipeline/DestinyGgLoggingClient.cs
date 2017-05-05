using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class DestinyGgLoggingClient : DestinyGgBaseClient {
    private readonly ILogger _logger;

    public DestinyGgLoggingClient(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService) : base(privateConstants, logger, timeService) {
      _logger = logger;
    }

    public override void Receive(string input) => _logger.LogInformation(input);

    public override void Send(string data) => _logger.LogInformation(data);

  }
}
