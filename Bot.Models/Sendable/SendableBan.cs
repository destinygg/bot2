using System;
using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models.Sendable {
  [DebuggerDisplay("Muted {Target} for {Duration.TotalMinutes}m for: {Reason}")]
  public class SendableBan : ISendable<Ban> {
    public SendableBan(Civilian target, TimeSpan duration) {
      Transmission = new Ban(target, duration);
    }

    public SendableBan(Civilian target, TimeSpan duration, string reason) {
      Transmission = new Ban(target, duration, reason);
    }

    public Ban Transmission { get; }
    public IUser Target => Transmission.Target;
    public TimeSpan Duration => Transmission.Duration;
    public string Reason => Transmission.Reason;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
