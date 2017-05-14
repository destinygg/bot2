using System;
using System.Collections.Generic;
using Bot.Models.Sendable;

namespace Bot.Logic.Interfaces {
  public interface ITwitterManager {
    IEnumerable<string> LatestTweetFromDestiny();
    IEnumerable<string> LatestTweetFromAslan();
    void MonitorNewTweets(Action<IReadOnlyList<SendablePublicMessage>> send);
  }
}
