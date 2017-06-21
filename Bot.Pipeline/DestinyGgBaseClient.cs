using System;
using System.Threading;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace Bot.Pipeline {
  public abstract class DestinyGgBaseClient : BaseClient {

    private readonly ILogger _logger;
    private readonly ITimeService _timeService;
    private readonly IPipelineManager _pipelineManager;
    private DateTime _lastConnectedAt;

    protected readonly WebSocket Websocket;

    protected DestinyGgBaseClient(
      IPrivateConstants privateConstants,
      ILogger logger,
      ITimeService timeService,
      IPipelineManager pipelineManager
    ) : base(logger) {
      _logger = logger;
      _timeService = timeService;
      _pipelineManager = pipelineManager;
      _pipelineManager.SetSender(Send);
      Websocket = new WebSocket("ws://www.destiny.gg:9998/ws");
      Websocket.SetCookie(new Cookie("authtoken", privateConstants.BotWebsocketAuth));
      Websocket.OnMessage += WebsocketMessaged;
      Websocket.OnClose += WebsocketClosed;
      Websocket.OnError += WebsocketErrored;
      Websocket.OnOpen += WebsocketOpened;
    }

    public override void Connect() {
      if (_lastConnectedAt - _timeService.UtcNow > TimeSpan.FromSeconds(MaximumBackoffTimeInSeconds)) {
        ConnectionFailureCount = 0;
      }
      while (!Websocket.IsAlive) {
        try {
          Websocket.Close();
          Websocket.Connect();
        } catch (Exception e) {
          _logger.LogError($"{nameof(DestinyGgBaseClient)} had an error connecting.", e);
        } finally {
          if (!Websocket.IsAlive) {
            _onConnectionFailure();
          }
          Thread.Sleep(TimeSpan.FromSeconds(1));
        }
      }
    }

    private void WebsocketMessaged(object sender, MessageEventArgs e) {
      LatestReceivedAt = _timeService.UtcNow;
      _pipelineManager.Enqueue(e.Data);
    }

    private void WebsocketOpened(object sender, EventArgs e) {
      _lastConnectedAt = _timeService.UtcNow;
      _logger.LogInformation("Connected!");
    }

    private void WebsocketClosed(object sender, EventArgs e) {
      _logger.LogWarning("Websocket closed! Reconnecting...");
      Connect();
    }

    private void WebsocketErrored(object sender, ErrorEventArgs e) {
      _logger.LogError("Websocket error! Reconnecting...", e.Exception);
      Connect();
    }

  }
}
