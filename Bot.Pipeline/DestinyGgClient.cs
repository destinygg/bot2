using System;
using System.Threading;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace Bot.Pipeline {
  public class DestinyGgClient : IClient {
    private readonly ILogger _logger;
    private readonly WebSocket _websocket;

    public DestinyGgClient(IPrivateConstants privateConstants, ILogger logger) {
      _logger = logger;
      _websocket = new WebSocket("ws://www.destiny.gg:9998/ws");
      _websocket.SetCookie(new Cookie("authtoken", privateConstants.BotWebsocketAuth));
      _websocket.OnMessage += WebsocketMessaged;
      _websocket.OnClose += WebsocketClosed;
      _websocket.OnError += WebsocketErrored;
      _websocket.OnOpen += WebsocketOpened;
    }

    public void Connect() {
      var retryCount = 0;
      while (_websocket.ReadyState != WebSocketState.Open) {
        try {
          _websocket.Connect();
        } catch (Exception e) {
          var backoffTime = Math.Min((int) Math.Pow(2, retryCount), 30);
          _logger.LogError($"{nameof(DestinyGgClient)} had error connecting. {nameof(retryCount)} is {retryCount} and {nameof(backoffTime)} is {backoffTime}", e);
          Thread.Sleep(TimeSpan.FromSeconds(backoffTime));
          retryCount++;
        }
      }
    }

    public void Receive(string input) { }

    public void Send(string data) { }

    private void WebsocketMessaged(object sender, MessageEventArgs e) => Receive(e.Data);

    private void WebsocketOpened(object sender, EventArgs e) => _logger.LogInformation("Connected!");

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
