using System;
using Bot.Tools.Contracts;

namespace Bot.Models {
  class PublicMessageFromCivilian : ReceivedMessage {
    public PublicMessageFromCivilian(Civilian sender, string text, ITimeService timeService) : base(sender, text, timeService) { }
    public PublicMessageFromCivilian(Civilian sender, string text, DateTime timestamp) : base(sender, text, timestamp) { }

  }
}
