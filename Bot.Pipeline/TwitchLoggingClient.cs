using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using TwitchLib.Models.Client;

namespace Bot.Pipeline {
  public class TwitchLoggingClient : TwitchBaseClient {
    private readonly ILogger _logger;

    public TwitchLoggingClient(
      IFactory<ChatMessage, IReceived<IUser, ITransmittable>> twitchChatMessageParser,
      IPrivateConstants privateConstants,
      IPipelineManager pipelineManager,
      ITimeService timeService,
      ILogger logger
    ) : base(twitchChatMessageParser, privateConstants, pipelineManager, timeService, logger) {
      _logger = logger;
    }

    public override void Send(string data) => _logger.LogInformation(data);

  }
}
