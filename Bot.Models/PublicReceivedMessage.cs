using System;
using System.Diagnostics;
using Bot.Tools.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class PublicReceivedMessage : ReceivedMessage {
    public PublicReceivedMessage(string text, ITimeService timeService) : base(new User("SampleUser"), text, timeService) { }
    public PublicReceivedMessage(string text, DateTime timestamp) : base(new User(timestamp.ToShortTimeString()), text, timestamp) { }

  }
}
