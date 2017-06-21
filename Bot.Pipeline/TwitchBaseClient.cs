using System;
using System.Threading;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using TwitchLib;
using TwitchLib.Events.Client;
using TwitchLib.Models.Client;

namespace Bot.Pipeline {
  public abstract class TwitchBaseClient : BaseClient {
    private readonly IFactory<ChatMessage, IReceived<IUser, ITransmittable>> _twitchChatMessageParser;
    private readonly IPipelineManager _pipelineManager;
    private readonly ITimeService _timeService;
    private readonly ILogger _logger;
    private DateTime _lastConnectedAt;

    protected readonly TwitchClient Client;

    protected TwitchBaseClient(
      IFactory<ChatMessage, IReceived<IUser, ITransmittable>> twitchChatMessageParser,
      IPrivateConstants privateConstants,
      IPipelineManager pipelineManager,
      ITimeService timeService,
      ILogger logger
    ) : base(logger) {
      _twitchChatMessageParser = twitchChatMessageParser;
      _pipelineManager = pipelineManager;
      _timeService = timeService;
      _logger = logger;

      _pipelineManager.SetSender(Send);

      var credentials = new ConnectionCredentials(privateConstants.TwitchNick, privateConstants.TwitchOauth);
      Client = new TwitchClient(credentials, "Destiny");
      Client.OnJoinedChannel += OnJoinedChannel;
      Client.OnMessageReceived += OnMessageReceived;
      Client.OnDisconnected += OnDisconnected;
      Client.OnConnectionError += OnConnectionError;
      Client.OnConnected += OnConnected;
    }

    private void OnJoinedChannel(object sender, OnJoinedChannelArgs e) => _logger.LogInformation($"Joined {e.Channel}");

    public override void Connect() {
      if (_lastConnectedAt - _timeService.UtcNow > TimeSpan.FromSeconds(MaximumBackoffTimeInSeconds)) {
        ConnectionFailureCount = 0;
      }
      while (!Client.IsConnected) {
        try {
          Client.Disconnect();
          Client.Connect();
        } catch (Exception e) {
          _logger.LogError($"{nameof(TwitchBaseClient)} had an error connecting.", e);
        } finally {
          if (!Client.IsConnected) {
            _onConnectionFailure();
          }
          Thread.Sleep(TimeSpan.FromSeconds(1));
        }
      }
    }

    private void OnMessageReceived(object sender, OnMessageReceivedArgs e) {
      LatestReceivedAt = _timeService.UtcNow;
      _pipelineManager.Enqueue(_twitchChatMessageParser.Create(e.ChatMessage));
    }

    private void OnConnected(object sender, OnConnectedArgs e) {
      _lastConnectedAt = _timeService.UtcNow;
      _logger.LogInformation("Connected!");
    }

    private void OnDisconnected(object sender, OnDisconnectedArgs e) {
      _logger.LogWarning($"{nameof(TwitchBaseClient)} disconnected! Reconnecting...");
      Connect();
    }

    private void OnConnectionError(object sender, OnConnectionErrorArgs e) {
      _logger.LogError($"{nameof(TwitchBaseClient)} error! Reconnecting...", e.Error.Exception);
      Connect();
    }

  }
}
