using System;
using System.Collections.Generic;
using Bot.Models.Sendable;
using CoreTweet;

namespace Bot.Logic.Interfaces {
  public interface ITwitterManager {
    Tuple<IEnumerable<string>, Status> LatestTweetFromDestiny(string prefix);
    IEnumerable<string> LatestTweetFromAslan(string prefix);
    void MonitorNewTweets(Action<IReadOnlyList<SendablePublicMessage>> send);
  }
}
