using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class User : IUser {
    public User(string nick, bool isMod = false) {
      Nick = nick;
      IsMod = isMod;
    }

    // To ensure thread safety, this object should remain readonly.
    public string Nick { get; }
    public IEnumerable<string> Flair { get; }
    public bool IsMod { get; }
    public override string ToString() => IsMod ? $"{Nick}(Mod)" : Nick;
  }
}
