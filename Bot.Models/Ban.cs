using System;

namespace Bot.Models {
  public class Ban : Punishment {
    public Ban(Civilian target, TimeSpan duration) : base(target, duration) { }
    public Ban(Civilian target, TimeSpan duration, string reason) : base(target, duration, reason) { }
    public override string PastTense => "Banned";
  }
}
