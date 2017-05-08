using Bot.Models.Interfaces;

namespace Bot.Models {
  public abstract class User : IUser {
    protected User(string nick, bool isMod, bool isPunishable) {
      Nick = nick;
      IsMod = isMod;
      IsPunishable = isPunishable;
    }

    // To ensure thread safety, this object should remain readonly.
    public string Nick { get; }
    public bool IsMod { get; }
    public bool IsPunishable { get; }
  }
}
