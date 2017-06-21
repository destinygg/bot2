using System;
using System.Threading;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace Bot.Pipeline {
  public abstract class DestinyGgBaseClient : IClient {
    private const int MaximumBackoffTimeInSeconds = 60;
    private readonly ILogger _logger;
    private readonly ITimeService _timeService;
    private readonly IPipelineManager _pipelineManager;
    private DateTime _lastConnectedAt;
    private int _connectionFailureCount;

    protected readonly WebSocket Websocket;

    protected DestinyGgBaseClient(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService, IPipelineManager pipelineManager) {
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

    public void Connect() {
      if (_lastConnectedAt - _timeService.UtcNow > TimeSpan.FromSeconds(MaximumBackoffTimeInSeconds)) {
        _connectionFailureCount = 0;
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

    private void _onConnectionFailure() {
      _connectionFailureCount++;
      var backoffTimeInSeconds = Math.Min((int) Math.Pow(2, _connectionFailureCount) - 1, MaximumBackoffTimeInSeconds);
      _logger.LogInformation($"Unable to connect. {nameof(backoffTimeInSeconds)} is {backoffTimeInSeconds}. {nameof(_connectionFailureCount)} is {_connectionFailureCount}.");
      Thread.Sleep(TimeSpan.FromSeconds(backoffTimeInSeconds));
    }

    public abstract void Send(string data);

    public DateTime LatestReceivedAt { get; private set; }

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
