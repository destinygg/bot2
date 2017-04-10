using Bot.Models.Interfaces;

namespace Bot.Models.Sendable {
  public class SendablePublicMessage : ISendable<PublicMessage> {

    public SendablePublicMessage(string text) {
      Transmission = new PublicMessage(text);
    }

    public PublicMessage Transmission { get; }
    public string Text => Transmission.Text;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
    public override string ToString() => $"Sending: {Text}";
  }
}
