using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("Sending: {Text}")]
  public class SendablePublicMessage : Message, ISendable {
    public SendablePublicMessage(string text) : base(text) { }
  }
}
