using System;

namespace Bot.Models {
  public class Ban : Punishment {
    public Ban(Civilian target, TimeSpan duration, string reason = null) : base(target, duration, reason) { }
    public Ban(string target, TimeSpan duration, string reason = null) : base(target, duration, reason) { }
    public override string PastTense => "Banned";
  }
}
