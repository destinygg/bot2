using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using TwitchLib.Models.Client;

namespace Bot.Pipeline {
  public class TwitchSendingClient : TwitchBaseClient {

    public TwitchSendingClient(
      IFactory<ChatMessage, IReceived<IUser, ITransmittable>> twitchChatMessageParser,
      IPrivateConstants privateConstants,
      IPipelineManager pipelineManager,
      ITimeService timeService,
      ISettings settings,
      ILogger logger
    ) : base(twitchChatMessageParser, privateConstants, pipelineManager, timeService, settings, logger) { }

    public override void Send(string data) => Client.SendMessage(data);

  }
}
