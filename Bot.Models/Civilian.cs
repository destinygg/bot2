using System.Diagnostics;

namespace Bot.Models {
  [DebuggerDisplay("{Nick}")]
  public class Civilian : User {
    public Civilian(string nick) : base(nick, false) {

    }

  }
}
