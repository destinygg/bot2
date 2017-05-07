using Bot.Models.Interfaces;
using Bot.Models.Websockets;

namespace Bot.Models.Sendable {
  public class SendablePublicMessage : ISendable<PublicMessage> {

    public SendablePublicMessage(string text) {
      Transmission = new PublicMessage(text);
    }

    public PublicMessage Transmission { get; }
    public string Text => Transmission.Text;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
    public IDggJson Json => new Websockets.SendablePublicMessage(Text);
    public override string ToString() => $"Sending: {Text}";
  }
}
