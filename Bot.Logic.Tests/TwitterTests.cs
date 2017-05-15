﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Tests;
using Bot.Tools;
using Bot.Tools.Interfaces;
using CoreTweet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class TwitterTests {

    [TestMethod]
    public void TwitterManager_LatestTweetFromDestiny_RequiresManualCheckingForReasonablewResult() {
      var container = new TestContainerManager();
      var twitterManager = container.Container.GetInstance<ITwitterManager>();

      var latest = twitterManager.LatestTweetFromDestiny();

      Console.WriteLine(ObjectDumper.Dump(latest));
    }

    [TestMethod]
    public void TwitterStatusFormatter_FormattingLatest200Tweets_RequiresManualCheckingForReasonableResults() {
      var container = new TestContainerManager();
      var privateConstants = container.Container.GetInstance<IPrivateConstants>();
      var tokens = Tokens.Create(privateConstants.TwitterConsumerKey, privateConstants.TwitterConsumerSecret, privateConstants.TwitterAccessToken, privateConstants.TwitterAccessTokenSecret);
      var tweets = tokens.Statuses.UserTimeline("OmniDestiny", 200, tweet_mode: TweetMode.Extended);
      //var tweets2 = tokens.Statuses.UserTimeline("OmniDestiny", 200, tweet_mode: TweetMode.Extended, max_id: tweets.Skip(150).First().Id); // library can't parse for some reason
      var formatter = container.Container.GetInstance<IFactory<Status, string, IEnumerable<string>>>();

      var responses = tweets.Select(x => formatter.Create(x, ""));

      Console.WriteLine(ObjectDumper.Dump(responses));
    }

  }
}