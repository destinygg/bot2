using System;
using Bot.Models.Contracts;

namespace Bot.Models {
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
  }
}
