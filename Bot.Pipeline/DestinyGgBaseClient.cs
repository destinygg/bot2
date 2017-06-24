using System;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace Bot.Pipeline {
  public abstract class DestinyGgBaseClient : BaseClient {
    private readonly IPrivateConstants _privateConstants;

    protected WebSocket Websocket;

    protected DestinyGgBaseClient(
      IPrivateConstants privateConstants,
      ILogger logger,
      ISettings settings,
      ITimeService timeService,
      IPipelineManager pipelineManager
    ) : base(logger, settings, timeService, pipelineManager) {
      _privateConstants = privateConstants;

      pipelineManager.SetSender(Send);
      _constructWebsocket();
    }

    private void _constructWebsocket() {
      Websocket = new WebSocket("ws://www.destiny.gg:9998/ws");
      Websocket.SetCookie(new Cookie("authtoken", _privateConstants.BotWebsocketAuth));
      Websocket.OnMessage += WebsocketMessaged;
      Websocket.OnClose += WebsocketClosed;
      Websocket.OnError += WebsocketErrored;
      Websocket.OnOpen += WebsocketOpened;
    }

    protected override Func<bool> IsConnected => () => Websocket.IsAlive;

    protected override Action Connect => () => {
      _constructWebsocket();
      Websocket.Connect();
    };

    public override void Disconnect() => Websocket.Close();

    private void WebsocketMessaged(object sender, MessageEventArgs e) => Messaged(e.Data);

    private void WebsocketOpened(object sender, EventArgs e) => Connected();

    private void WebsocketClosed(object sender, EventArgs e) => Disconnected();

    private void WebsocketErrored(object sender, ErrorEventArgs e) => Errored(e.Exception);

  }
}
