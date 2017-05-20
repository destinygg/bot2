using System;
using Bot.Models.Interfaces;

namespace Bot.Models {
  public abstract class Punishment : ITargetable {

    protected Punishment(Civilian target, TimeSpan duration, string reason = null) {
      Target = target;
      Duration = duration;
      Reason = reason;
    }

    protected Punishment(string target, TimeSpan duration, string reason = null) :
      this(new Civilian(target), duration, reason) { }

    public IUser Target { get; }
    public TimeSpan Duration { get; }
    public string Reason { get; }
    public abstract string PastTense { get; }
    public override string ToString() {
      var reasonString = string.IsNullOrEmpty(Reason) ? "" : $" for: {Reason}";
      return $"{PastTense} {Target} for {Duration.TotalMinutes}m{reasonString}";
    }

  }
}
