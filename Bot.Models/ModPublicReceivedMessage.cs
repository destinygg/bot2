using System;
using System.Diagnostics;
using Bot.Tools.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class ModPublicReceivedMessage : ReceivedMessage {
    public ModPublicReceivedMessage(string text, ITimeService timeService) : base(new User("SampleMod", true), text, timeService) { }
    public ModPublicReceivedMessage(string text, DateTime timestamp) : base(new User(timestamp.ToShortTimeString(), true), text, timestamp) { }

  }
}
