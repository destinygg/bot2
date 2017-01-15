using System;
using System.Diagnostics;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class ModPublicReceivedMessage : ReceivedMessage {
    public ModPublicReceivedMessage(string text) : base(new User("SampleMod", true), text) { }
    public ModPublicReceivedMessage(string text, DateTime timestamp) : base(new User(timestamp.ToShortTimeString(), true), text, timestamp) { }

  }
}
