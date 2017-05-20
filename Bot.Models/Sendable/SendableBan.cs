using System;
using Bot.Models.Interfaces;
using Bot.Models.Websockets;

namespace Bot.Models.Sendable {
  public class SendableBan : ISendable<Ban> {

    public SendableBan(Civilian target, TimeSpan duration, string reason = null) {
      Transmission = new Ban(target, duration, reason);
    }

    public SendableBan(string target, TimeSpan duration, string reason = null) :
      this(new Civilian(target), duration, reason) { }

    public Ban Transmission { get; }
    public IUser Target => Transmission.Target;
    public TimeSpan Duration => Transmission.Duration;
    public string Reason => Transmission.Reason;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
    public IDggJson Json => new Websockets.SendableBan(Target.Nick, false, Duration, Duration == TimeSpan.MaxValue, Reason);
    public override string ToString() => $"Banned {Target} for {Duration.TotalMinutes}m for: {Reason}";
  }
}
