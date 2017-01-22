using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class User : IUser {
    protected User(string nick, bool isMod) {
      Nick = nick;
      IsMod = isMod;
    }

    // To ensure thread safety, this object should remain readonly.
    public string Nick { get; }
    public IEnumerable<string> Flair { get; }
    public bool IsMod { get; }

  }
}
