using Bot.Models.Interfaces;
using Bot.Models.Websockets;

namespace Bot.Models.Sendable {
  public class SendablePrivateMessage : ISendable<PrivateMessage> {

    public SendablePrivateMessage(string text, Moderator moderator) {
      Transmission = new PrivateMessage(text, moderator);
    }

    public PrivateMessage Transmission { get; }
    public string Text => Transmission.Text;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
    public IDggJson Json => new Websockets.SendablePrivateMessage(Text, Transmission.Target.Nick);
    public string Twitch => Text;
    public override string ToString() => $"Sending: {Text}";
  }
}
