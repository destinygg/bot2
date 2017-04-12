using System;

namespace Bot.Models {
  public class Mute : Punishment {
    public Mute(Civilian target, TimeSpan duration) : base(target, duration) { }
    public Mute(Civilian target, TimeSpan duration, string reason) : base(target, duration, reason) { }
    public override string PastTense => "Muted";
  }
}
