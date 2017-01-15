using System.Diagnostics;
using Bot.Models.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("Sending UnMuteBan targeting: {Target})")]
  public class SendableUnMuteBan : UnMuteBan, ISendable {
    public SendableUnMuteBan(IUser target) : base(target) { }
  }
}
