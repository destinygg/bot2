using System.Diagnostics;
using Bot.Models.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("Sending: {Text}")]
  public class SendablePublicMessage : Message, ISendable {
    public SendablePublicMessage(string text) : base(text) { }
  }
}
