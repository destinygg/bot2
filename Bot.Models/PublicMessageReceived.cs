using System;
using System.Diagnostics;
using Bot.Models.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick } Saying:{Text}")]
  public class PublicMessageReceived : PublicMessage, IPublicMessageReceived {

    public DateTime Timestamp { get; }
    public IUser Sender { get; }

    public PublicMessageReceived(string text) : base(text) {
      Sender = new User("TestUser");
      Timestamp = DateTime.UtcNow;
    }

    public PublicMessageReceived(string text, bool isMod) : this(text) {
      Sender = new User("TestMod", isMod);
      Timestamp = DateTime.UtcNow;
    }
    public PublicMessageReceived(bool isBlank) : this("") {
      Sender = new User("");
      Timestamp = new DateTime(1970, 1, 1);
    }
  }
}
