using Bot.Models.Contracts;

namespace Bot.Models {
  public class SendableMessage : Message, ISendable {
    public SendableMessage(string text) : base(text) { }
  }
}
