using System;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;

namespace Bot.Pipeline {
  public abstract class TwitchBaseClient : BaseClient {
    private readonly IFactory<ChatMessage, IReceived<IUser, ITransmittable>> _twitchChatMessageParser;
    private readonly IPrivateConstants _privateConstants;
    private readonly ILogger _logger;

    protected TwitchClient Client;

    protected TwitchBaseClient(
      IFactory<ChatMessage, IReceived<IUser, ITransmittable>> twitchChatMessageParser,
      IPrivateConstants privateConstants,
      IPipelineManager pipelineManager,
      ITimeService timeService,
      ISettings settings,
      ILogger logger
    ) : base(logger, settings, timeService, pipelineManager) {
      _twitchChatMessageParser = twitchChatMessageParser;
      _privateConstants = privateConstants;
      _logger = logger;

      pipelineManager.SetSender(Send);
      _constructClient();
    }

    private void _constructClient() {
      var credentials = new ConnectionCredentials(_privateConstants.TwitchNick, _privateConstants.TwitchOauth);
      Client = new TwitchClient(credentials, "Destiny");
      Client.OnJoinedChannel += OnJoinedChannel;
      Client.OnMessageReceived += OnMessageReceived;
      Client.OnDisconnected += OnDisconnected;
      Client.OnConnectionError += OnConnectionError;
      Client.OnConnected += OnConnected;
    }

    private void OnJoinedChannel(object sender, OnJoinedChannelArgs e) => _logger.LogInformation($"Joined {e.Channel}");

    protected override Func<bool> IsConnected => () => Client.IsConnected;

    protected override Action Connect => () => {
      _constructClient();
      Client.Connect();
    };

    public override void Disconnect() => Client.Disconnect();

    private void OnMessageReceived(object sender, OnMessageReceivedArgs e) => Messaged(_twitchChatMessageParser.Create(e.ChatMessage));

    private void OnConnected(object sender, OnConnectedArgs e) => Connected();

    private void OnDisconnected(object sender, OnDisconnectedArgs e) => Disconnected();

    private void OnConnectionError(object sender, OnConnectionErrorArgs e) => Errored(e.Error.Exception);
  }
}
