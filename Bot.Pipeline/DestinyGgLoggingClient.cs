using System.Collections.Generic;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class DestinyGgLoggingClient : DestinyGgBaseClient, ICommandHandler<IEnumerable<string>> {
    private readonly ILogger _logger;

    public DestinyGgLoggingClient(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService) : base(privateConstants, logger, timeService) {
      _logger = logger;
    }

    public override void Receive(string input) => _logger.LogInformation(input);

    public override void Send(string data) => _logger.LogInformation(data);

    public void Handle(IEnumerable<string> commands) {
      foreach (var command in commands) {
        Send(command);
      }
    }

  }
}
