using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class DestinyGgSendingClient : DestinyGgBaseClient {

    public DestinyGgSendingClient(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService, IPipelineManager pipelineManager) : base(privateConstants, logger, timeService, pipelineManager) { }

    public override void Send(string data) => Websocket.Send(data);

  }
}
