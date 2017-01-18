using System.Diagnostics;
using Bot.Models.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("Sending UnMuteBan targeting: {Target})")]
  public class SendablePardon : Pardon, ISendable {
    public SendablePardon(IUser target) : base(target) { }
  }
}
