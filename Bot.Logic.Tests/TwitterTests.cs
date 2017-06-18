using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Main.Moderate;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using CoreTweet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class TwitterTests {

    [TestMethod]
    public void TwitterManager_LatestTweetFromDestiny_RequiresManualCheckingForReasonablewResult_DoNotRunContinuously() {
      var container = new TestContainerManager();
      var twitterManager = container.Container.GetInstance<ITwitterManager>();

      var latest = twitterManager.LatestTweetFromDestiny();

      Console.WriteLine(ObjectDumper.Dump(latest));
    }

    [TestMethod]
    public void TwitterStatusFormatter_FormattingLatest200Tweets_RequiresManualCheckingForReasonableResults_DoNotRunContinuously() {
      var container = new TestContainerManager();
      var privateConstants = container.Container.GetInstance<IPrivateConstants>();
      var tokens = Tokens.Create(privateConstants.TwitterConsumerKey, privateConstants.TwitterConsumerSecret, privateConstants.TwitterAccessToken, privateConstants.TwitterAccessTokenSecret);
      var tweets = tokens.Statuses.UserTimeline("OmniDestiny", 200, tweet_mode: TweetMode.Extended);
      //var tweets2 = tokens.Statuses.UserTimeline("OmniDestiny", 200, tweet_mode: TweetMode.Extended, max_id: tweets.Skip(150).First().Id); // library can't parse for some reason
      var formatter = container.Container.GetInstance<IFactory<Status, string, IEnumerable<string>>>();

      var responses = tweets.Select(x => formatter.Create(x, ""));

      Console.WriteLine(ObjectDumper.Dump(responses));
    }

    [TestMethod]
    public void TwitterManager_LatestTweetFromDestiny_StoredToDb_DoNotRunContinuously() {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var twitterManager = container.GetInstance<ITwitterManager>();
      var unitOfWork = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var id = unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
      Assert.AreEqual(-1, id);

      twitterManager.LatestTweetFromDestiny();

      id = unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
      Assert.AreNotEqual(-1, id);
    }

  }
}
