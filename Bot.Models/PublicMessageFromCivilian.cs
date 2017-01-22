using System;
using System.Diagnostics;
using Bot.Tools.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("{Sender}: {Text}")]
  public class PublicMessageFromCivilian : MessageFromCivilian {
    public PublicMessageFromCivilian(string text, ITimeService timeService) : base(new Civilian("SampleUser"), text, timeService) { }
    public PublicMessageFromCivilian(string text, DateTime timestamp) : base(new Civilian(timestamp.ToShortTimeString()), text, timestamp) { }

  }
}
