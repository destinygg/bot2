using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class Mute : Punishment {
    protected Mute(IUser target, TimeSpan duration) : base(target, duration) { }
    protected Mute(IUser target, TimeSpan duration, string reason) : base(target, duration, reason) { }

  }
}
