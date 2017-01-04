using System;
using System.Diagnostics;
using Bot.Models.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class ReceivedPublicMessage : Message, IPublicMessageReceived {

    public ReceivedPublicMessage(string text) : base(text) {
      Sender = new User("TestUser");
      Timestamp = DateTime.UtcNow;
    }

    public ReceivedPublicMessage(string text, bool isMod) : this(text) {
      Sender = new User("TestMod", isMod);
      Timestamp = DateTime.UtcNow;
    }

    public ReceivedPublicMessage(bool isBlank) : this("") {
      Sender = new User("");
      Timestamp = new DateTime(1970, 1, 1);
    }

    // To ensure thread safety, this object should remain readonly.
    public DateTime Timestamp { get; }
    public IUser Sender { get; }
    public bool FromMod => Sender.IsMod;
  }
}
