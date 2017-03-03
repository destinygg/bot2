using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models.Sendable {
  [DebuggerDisplay("Sending: {Text}")]
  public class SendableError : ISendable<ErrorMessage> {

    public SendableError(string text) {
      Transmission = new ErrorMessage(text);
    }

    public ErrorMessage Transmission { get; }
    public string Text => Transmission.Text;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
