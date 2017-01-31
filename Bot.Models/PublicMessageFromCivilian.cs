using System;
using System.Diagnostics;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("{Sender}: {Text}")]
  public class PublicMessageFromCivilian : ReceivedMessage<Civilian, PublicMessage> {
    public PublicMessageFromCivilian(string text, ITimeService timeService) : base(new Civilian("SampleUser"), new PublicMessage(text), timeService) { }
    public PublicMessageFromCivilian(string text, DateTime timestamp) : base(new Civilian(timestamp.ToShortTimeString()), new PublicMessage(text), timestamp) { }

  }
}
