using System;
using System.Collections.Generic;
using Bot.Models.Sendable;
using CoreTweet;

namespace Bot.Logic.Interfaces {
  public interface ITwitterManager {
    Tuple<IEnumerable<string>, Status> LatestTweetFromDestiny(bool isJustTweeted);
    IEnumerable<string> LatestTweetFromAslan();
    void MonitorNewTweets(Action<IReadOnlyList<SendablePublicMessage>> send);
  }
}
