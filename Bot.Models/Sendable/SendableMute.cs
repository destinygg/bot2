using System;
using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models.Sendable {
  [DebuggerDisplay("Muted {Target} for {Duration.TotalMinutes}m for: {Reason}")]
  public class SendableMute : ISendable<Mute> {
    public SendableMute(Civilian target, TimeSpan duration) {
      Transmission = new Mute(target, duration);
    }

    public SendableMute(Civilian target, TimeSpan duration, string reason) {
      Transmission = new Mute(target, duration, reason);
    }

    public Mute Transmission { get; }
    public IUser Target => Transmission.Target;
    public TimeSpan Duration => Transmission.Duration;
    public string Reason => Transmission.Reason;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
