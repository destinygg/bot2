using System;
using System.Collections.Generic;
using Bot.Models.Sendable;
using CoreTweet;

namespace Bot.Logic.Interfaces {
  public interface ITwitterManager {
    Status LatestTweetFromDestiny();
    Status LatestTweetFromAslan();
    void MonitorNewTweets(Action<IReadOnlyList<SendablePublicMessage>> send);
  }
}
