using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using CoreTweet;
using CoreTweet.Streaming;

namespace Bot.Logic {
  public class TwitterStreamingMessageObserver : ITwitterStreamingMessageObserver {
    private readonly ILogger _logger;
    private readonly IFactory<StreamingMessage, Status> _statusFactory;
    private readonly IFactory<Status, string, IEnumerable<string>> _twitterStatusFormatter;
    private Action _reconnect;
    private Action<IReadOnlyList<SendablePublicMessage>> _send;

    public TwitterStreamingMessageObserver(ILogger logger, IFactory<StreamingMessage, Status> statusFactory, IFactory<Status, string, IEnumerable<string>> twitterStatusFormatter) {
      _logger = logger;
      _statusFactory = statusFactory;
      _twitterStatusFormatter = twitterStatusFormatter;
    }

    public void SetReconnect(Action reconnect) {
      _reconnect = reconnect;
    }

    public void SetSend(Action<IReadOnlyList<SendablePublicMessage>> send) {
      _send = send;
    }

    public void OnNext(StreamingMessage streamingMessage) {
      if (streamingMessage.Type == MessageType.Create) {
        var status = _statusFactory.Create(streamingMessage);
        var formatted = _twitterStatusFormatter.Create(status, "twitter.com/OmniDestiny just tweeted: ");
        _send(formatted.Select(x => new SendablePublicMessage(x)).ToList());
      }
    }

    public void OnError(Exception error) {
      _logger.LogError($"The {nameof(TwitterStreamingMessageObserver)} got an error", error);
      _reconnect();
    }

    public void OnCompleted() {
      _logger.LogError($"The {nameof(TwitterStreamingMessageObserver)} reached completion, which is unexpected.");
      _reconnect();
    }

  }
}
