using System.Diagnostics;

namespace Bot.Models {
  [DebuggerDisplay("{Nick}(Mod)")]
  public class Moderator : User {
    public Moderator(string nick) : base(nick, true) {
      
    }

  }
}
