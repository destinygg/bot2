using System;
using System.Diagnostics;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class PublicReceivedMessage : ReceivedMessage {
    public PublicReceivedMessage(string text) : base(new User("SampleUser"), text) { }
    public PublicReceivedMessage(string text, DateTime timestamp) : base(new User(timestamp.ToShortTimeString()), text, timestamp) { }

  }
}
