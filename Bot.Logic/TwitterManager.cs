using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using CoreTweet;

namespace Bot.Logic {
  public class TwitterManager : ITwitterManager {
    private readonly IPrivateConstants _privateConstants;
    private readonly ILogger _logger;
    private readonly ITimeService _timeService;
    private readonly ITwitterStreamingMessageObserver _twitterObserver;
    private readonly IFactory<Status, string, IEnumerable<string>> _twitterStatusFormatter;
    private IDisposable _twitterStream;

    public TwitterManager(IPrivateConstants privateConstants, ILogger logger, ITimeService timeService, ITwitterStreamingMessageObserver twitterObserver, IFactory<Status, string, IEnumerable<string>> twitterStatusFormatter) {
      _privateConstants = privateConstants;
      _logger = logger;
      _timeService = timeService;
      _twitterObserver = twitterObserver;
      _twitterStatusFormatter = twitterStatusFormatter;
    }

    public IEnumerable<string> LatestTweetFromDestiny() => LatestTweet("OmniDestiny");

    public IEnumerable<string> LatestTweetFromAslan() => LatestTweet("AslanVondran");

    public void MonitorNewTweets(Action<IReadOnlyList<SendablePublicMessage>> send) {
      _logger.LogInformation("Monitoring new tweets...");
      _twitterStream?.Dispose();
      var tokens = Tokens.Create(_privateConstants.TwitterConsumerKey, _privateConstants.TwitterConsumerSecret, _privateConstants.TwitterAccessToken, _privateConstants.TwitterAccessTokenSecret);
      _twitterObserver.SetReconnect(() => MonitorNewTweets(send));
      _twitterObserver.SetSend(send);
      _twitterStream = tokens.Streaming.UserAsObservable().Subscribe(_twitterObserver); // make sure this uses TweetMode.Extended
    }

    private IEnumerable<string> LatestTweet(string twitterHandle) {
      var tokens = Tokens.Create(_privateConstants.TwitterConsumerKey, _privateConstants.TwitterConsumerSecret, _privateConstants.TwitterAccessToken, _privateConstants.TwitterAccessTokenSecret);
      var status = tokens.Statuses.UserTimeline(twitterHandle, 1, tweet_mode: TweetMode.Extended).First();
      var delta = (_timeService.UtcNow - status.CreatedAt.UtcDateTime).ToPretty(_logger);
      return _twitterStatusFormatter.Create(status, $"twitter.com/{twitterHandle} {delta} ago: ");
    }

  }
}
