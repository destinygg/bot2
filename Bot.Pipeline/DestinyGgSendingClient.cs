using System;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class DestinyGgSendingClient : DestinyGgBaseClient {
    private readonly ILogger _logger;
    private Action<string> _receiveAction;

    public DestinyGgSendingClient(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService) : base(privateConstants, logger, timeService) {
      _logger = logger;
    }

    public override void Receive(string input) {
      _receiveAction(input);
      _logger.LogInformation(input);
    }

    public override void Send(string data) => Websocket.Send(data);

    public override void SetReceive(Action<string> receiveAction) => _receiveAction = receiveAction;

  }
}
