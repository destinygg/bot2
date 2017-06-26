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
    private readonly ITwitterStreamingMessageObserver _twitterObserver;
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly IFactory<Status, string, IEnumerable<string>> _twitterStatusFormatter;
    private IDisposable _twitterStream;

    public TwitterManager(
      IPrivateConstants privateConstants,
      ILogger logger,
      ITwitterStreamingMessageObserver twitterObserver,
      IQueryCommandService<IUnitOfWork> unitOfWork,
      IFactory<Status, string, IEnumerable<string>> twitterStatusFormatter
    ) {
      _privateConstants = privateConstants;
      _logger = logger;
      _twitterObserver = twitterObserver;
      _unitOfWork = unitOfWork;
      _twitterStatusFormatter = twitterStatusFormatter;
    }

    public Tuple<IEnumerable<string>, Status> LatestTweetFromDestiny(string prefix) {
      var status = _getLatestStatus("OmniDestiny");
      _unitOfWork.Command(u => u.StateIntegers.LatestDestinyTweetId = status.Id);
      var formatted = _twitterStatusFormatter.Create(status, prefix);
      return Tuple.Create(formatted, status);
    }

    public IEnumerable<string> LatestTweetFromAslan(string prefix) => _getLatestStatus("AslanVondran").Apply(s => _twitterStatusFormatter.Create(s, prefix));

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
