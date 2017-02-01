using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("Sending: {Text}")]
  public class SendablePublicMessage : ISendable<PublicMessage> {

    public SendablePublicMessage(string text) {
      Transmission = new PublicMessage(text);
    }

    public PublicMessage Transmission { get; }
    public string Text => Transmission.Text;
  }
}
