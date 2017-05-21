using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
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
      ILogger logger
    ) : base(twitchChatMessageParser, privateConstants, pipelineManager, timeService, logger) { }

    public override void Send(string data) => Client.SendMessage(data);

  }
}
