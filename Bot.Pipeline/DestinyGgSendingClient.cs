using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class DestinyGgSendingClient : DestinyGgBaseClient {

    public DestinyGgSendingClient(
      IPrivateConstants privateConstants,
      ISettings settings,
      ILogger logger,
      ITimeService timeService,
      IPipelineManager pipelineManager
    ) : base(privateConstants, logger, settings, timeService, pipelineManager) { }

    public override void Send(string data) => Websocket.Send(data);

  }
}
