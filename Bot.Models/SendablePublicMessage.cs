using Bot.Models.Contracts;

namespace Bot.Models {
  public class SendablePublicMessage : Message, ISendable {
    public SendablePublicMessage(string text) : base(text) { }
    public override string ToString() => $"Sending a public message: {Text}";
  }
}
