using System;
using Bot.Models.Interfaces;
using Bot.Models.Websockets;

namespace Bot.Models.Sendable {
  public class SendableIpban : ISendable<Ipban> {

    public SendableIpban(Civilian target, TimeSpan duration, string reason = null) {
      Transmission = new Ipban(target, duration, reason);
    }

    public SendableIpban(string target, TimeSpan duration, string reason = null) :
      this(new Civilian(target), duration, reason) { }

    public Ipban Transmission { get; }
    public IUser Target => Transmission.Target;
    public TimeSpan Duration => Transmission.Duration;
    public string Reason => Transmission.Reason;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
    public IDggJson Json => new Websockets.SendableBan(Target.Nick, true, Duration, Duration == TimeSpan.MaxValue, Reason);
    public string Twitch => Duration == TimeSpan.MaxValue ? $".ban {Target} {Reason}" : $".timeout {Target} {Duration.TotalSeconds} {Reason}";
    public override string ToString() => $"Ipbanned {Target} for {Duration.TotalMinutes}m for: {Reason}";
  }
}
