using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
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
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private IDisposable _twitterStream;

    public TwitterManager(
      IPrivateConstants privateConstants,
      ILogger logger,
      ITimeService timeService,
      ITwitterStreamingMessageObserver twitterObserver,
      IFactory<Status, string, IEnumerable<string>> twitterStatusFormatter,
      IQueryCommandService<IUnitOfWork> unitOfWork
    ) {
      _privateConstants = privateConstants;
      _logger = logger;
      _timeService = timeService;
      _twitterObserver = twitterObserver;
      _twitterStatusFormatter = twitterStatusFormatter;
      _unitOfWork = unitOfWork;
    }

    public IEnumerable<string> LatestTweetFromDestiny() {
      var status = _getLatestStatus("OmniDestiny");
      _unitOfWork.Command(u => u.StateIntegers.LatestDestinyTweetId = status.Id);
      return _format(status);
    }

    public IEnumerable<string> LatestTweetFromAslan() => _getLatestStatus("AslanVondran").Apply(_format);

    public void MonitorNewTweets(Action<IReadOnlyList<SendablePublicMessage>> send) {
      _logger.LogInformation("Monitoring new tweets...");
      _twitterStream?.Dispose();
      var tokens = Tokens.Create(_privateConstants.TwitterConsumerKey, _privateConstants.TwitterConsumerSecret, _privateConstants.TwitterAccessToken, _privateConstants.TwitterAccessTokenSecret);
      _twitterObserver.SetReconnect(() => MonitorNewTweets(send));
      _twitterObserver.SetSend(send);
      _twitterStream = tokens.Streaming.UserAsObservable().Subscribe(_twitterObserver); // https://dev.twitter.com/rest/reference/get/statuses/user_timeline Todo Investigate periodic polling; the rate limiting isn't too bad
    }

    private Status _getLatestStatus(string twitterHandle) {
      var tokens = Tokens.Create(_privateConstants.TwitterConsumerKey, _privateConstants.TwitterConsumerSecret, _privateConstants.TwitterAccessToken, _privateConstants.TwitterAccessTokenSecret);
      return tokens.Statuses.UserTimeline(twitterHandle, 1, tweet_mode: TweetMode.Extended).First();
    }

    private IEnumerable<string> _format(Status status) {
      var delta = (_timeService.UtcNow - status.CreatedAt.UtcDateTime).ToPretty(_logger);
      return _twitterStatusFormatter.Create(status, $"twitter.com/{status.User.ScreenName} {delta} ago: ");
    }

  }
}
