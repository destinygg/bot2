using Bot.Models.Interfaces;
using Bot.Models.Websockets;

namespace Bot.Models.Sendable {
  public class SendableError : ISendable<ErrorMessage> {

    public SendableError(string text) {
      Transmission = new ErrorMessage(text);
    }

    public ErrorMessage Transmission { get; }
    public string Text => Transmission.Text;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
    public IDggJson Json => new Websockets.SendablePublicMessage(Text);
    public string Twitch => $"Internal Error: {Text}";
    public override string ToString() => $"Sending: {Text}";
  }
}
