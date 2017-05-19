using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class DestinyGgSendingClient : DestinyGgBaseClient {

    public DestinyGgSendingClient(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService) : base(privateConstants, logger, timeService) { }

    public override void Send(string data) => Websocket.Send(data);

  }
}
