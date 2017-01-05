using System;
using System.Diagnostics;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class PublicReceivedMessage : ReceivedMessage {
    public PublicReceivedMessage(string text) : base(text) {
      Sender = new User("SampleUser");
      Timestamp = DateTime.UtcNow;
    }

  }
}
