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
    private DateTime _lastConnectedAt;
    private int _connectionAttemptedCount;

    protected readonly WebSocket Websocket;

    public DestinyGgBaseClient(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService) {
      _logger = logger;
      _timeService = timeService;
      Websocket = new WebSocket("ws://www.destiny.gg:9998/ws");
      Websocket.SetCookie(new Cookie("authtoken", privateConstants.BotWebsocketAuth));
      Websocket.OnMessage += WebsocketMessaged;
      Websocket.OnClose += WebsocketClosed;
      Websocket.OnError += WebsocketErrored;
      Websocket.OnOpen += WebsocketOpened;
    }

    public void Connect() {
      if (_lastConnectedAt - _timeService.UtcNow > TimeSpan.FromSeconds(MaximumBackoffTimeInSeconds)) {
        _connectionAttemptedCount = 0;
      }
      while (Websocket.ReadyState != WebSocketState.Open) {
        try {
          var backoffTimeInSeconds = Math.Min((int) Math.Pow(2, _connectionAttemptedCount) - 1, MaximumBackoffTimeInSeconds);
          Thread.Sleep(TimeSpan.FromSeconds(backoffTimeInSeconds));
          _logger.LogInformation($"Connecting... {nameof(backoffTimeInSeconds)} is {backoffTimeInSeconds}. {nameof(_connectionAttemptedCount)} is {_connectionAttemptedCount}.");
          Websocket.Connect();
          _connectionAttemptedCount++;
        } catch (Exception e) {
          _logger.LogError($"{nameof(DestinyGgBaseClient)} had an error connecting.", e);
        }
      }
    }

    public abstract void Receive(string input);

    public virtual void SetReceive(Action<string> receiveAction) { }

    public abstract void Send(string data);

    private void WebsocketMessaged(object sender, MessageEventArgs e) => Receive(e.Data);

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
