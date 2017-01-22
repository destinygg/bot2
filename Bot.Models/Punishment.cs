using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class Punishment : ITargetable {
    protected Punishment(Civilian target, TimeSpan duration) {
      Target = target;
      Duration = duration;
    }

    protected Punishment(Civilian target, TimeSpan duration, string reason) {
      Target = target;
      Duration = duration;
      Reason = reason;
    }

    public IUser Target { get; }
    public TimeSpan Duration { get; }
    public string Reason { get; }

  }
}
