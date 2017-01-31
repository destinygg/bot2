using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("Pardoned {Target})")]
  public class SendablePardon : Pardon, ISendable {
    public SendablePardon(IUser target) : base(target) { }
  }
}
