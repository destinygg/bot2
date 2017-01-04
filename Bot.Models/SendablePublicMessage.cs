using Bot.Models.Contracts;

namespace Bot.Models {
  public class SendablePublicMessage : Message, IPublicMessage {
    public SendablePublicMessage(string text) : base(text) { }
  }
}
