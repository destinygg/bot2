using System;

namespace Bot.Models {
  public abstract class Mute : Punishment {
    protected Mute(Civilian target, TimeSpan duration) : base(target, duration) { }
    protected Mute(Civilian target, TimeSpan duration, string reason) : base(target, duration, reason) { }

  }
}
