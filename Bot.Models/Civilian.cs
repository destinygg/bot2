using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("{Nick}")]
  public class Civilian : User {
    public Civilian(string nick) : base(nick, false) {

    }

  }
}
