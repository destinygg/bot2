using System;

namespace Bot.Models {
  public class Mute : Punishment {
    public Mute(Civilian target, TimeSpan duration, string reason = null) : base(target, duration, reason) { }
    public Mute(string target, TimeSpan duration, string reason = null) : base(target, duration, reason) { }
    public override string PastTense => "Muted";
  }
}
