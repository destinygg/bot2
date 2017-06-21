using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using CoreTweet;

namespace Bot.Logic {
  public class TwitterManager : ITwitterManager {
    private readonly IPrivateConstants _privateConstants;
    private readonly ILogger _logger;
    private readonly ITwitterStreamingMessageObserver _twitterObserver;
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private IDisposable _twitterStream;

    public TwitterManager(
      IPrivateConstants privateConstants,
      ILogger logger,
      ITwitterStreamingMessageObserver twitterObserver,
      IQueryCommandService<IUnitOfWork> unitOfWork
    ) {
      _privateConstants = privateConstants;
      _logger = logger;
      _twitterObserver = twitterObserver;
      _unitOfWork = unitOfWork;
    }

    public Status LatestTweetFromDestiny() {
      var status = _getLatestStatus("OmniDestiny");
      _unitOfWork.Command(u => u.StateIntegers.LatestDestinyTweetId = status.Id);
      return status;
    }

    public Status LatestTweetFromAslan() => _getLatestStatus("AslanVondran");

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

  }
}
