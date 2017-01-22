using System;
using System.Diagnostics;
using Bot.Tools.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("PublicFrom:{Sender} Saying:{Text}")]
  public class PublicReceivedMessage : ReceivedMessage {
    public PublicReceivedMessage(string text, ITimeService timeService) : base(new Civilian("SampleUser"), text, timeService) { }
    public PublicReceivedMessage(string text, DateTime timestamp) : base(new Civilian(timestamp.ToShortTimeString()), text, timestamp) { }

  }
}
