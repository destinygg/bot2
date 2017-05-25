using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using CoreTweet;
using CoreTweet.Streaming;

namespace Bot.Logic {
  public class TwitterStreamingMessageObserver : ITwitterStreamingMessageObserver {
    private readonly ILogger _logger;
    private readonly IFactory<StreamingMessage, Status> _statusFactory;
    private readonly IFactory<Status, string, IEnumerable<string>> _twitterStatusFormatter;
    private readonly IPrivateConstants _privateConstants;
    private Action _reconnect;
    private Action<IReadOnlyList<SendablePublicMessage>> _send;

    public TwitterStreamingMessageObserver(ILogger logger, IFactory<StreamingMessage, Status> statusFactory, IFactory<Status, string, IEnumerable<string>> twitterStatusFormatter, IPrivateConstants privateConstants) {
      _logger = logger;
      _statusFactory = statusFactory;
      _twitterStatusFormatter = twitterStatusFormatter;
      _privateConstants = privateConstants;
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
        var tokens = Tokens.Create(_privateConstants.TwitterConsumerKey, _privateConstants.TwitterConsumerSecret, _privateConstants.TwitterAccessToken, _privateConstants.TwitterAccessTokenSecret);
        var extendedStatus = tokens.Statuses.Lookup(status.Id.Wrap(), tweet_mode: TweetMode.Extended).Single();
        var formatted = _twitterStatusFormatter.Create(extendedStatus, "twitter.com/OmniDestiny just tweeted: ");
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
