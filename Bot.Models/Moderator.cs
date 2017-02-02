using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("{Nick}(Mod)")]
  public class Moderator : User {
    public Moderator(string nick) : base(nick, true) {
      
    }

    public override T Accept<T>(IUserVisitor<T> visitor) => visitor.Visit(this);
  }
}
