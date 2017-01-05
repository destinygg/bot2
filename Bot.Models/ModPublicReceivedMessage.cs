using System;
using System.Diagnostics;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class ModPublicReceivedMessage : ReceivedMessage {
    public ModPublicReceivedMessage(string text) : base(text) {
      Sender = new User("SampleMod", true);
      Timestamp = DateTime.UtcNow;
    }

  }
}
