using Bot.Models.Contracts;

namespace Bot.Models {
  public class SendableMessage : Message, ISendable {
    public SendableMessage(string text) : base(text) { }
    public override string ToString() => $"Sending a public message: {Text}";
  }
}
