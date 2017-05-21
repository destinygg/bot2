using System;
using Bot.Models.Interfaces;
using Bot.Models.Websockets;

namespace Bot.Models.Sendable {
  public class SendableMute : ISendable<Mute> {

    public SendableMute(Civilian target, TimeSpan duration, string reason = null) {
      Transmission = new Mute(target, duration, reason);
    }

    public SendableMute(string target, TimeSpan duration, string reason = null) :
      this(new Civilian(target), duration, reason) { }

    public Mute Transmission { get; }
    public IUser Target => Transmission.Target;
    public TimeSpan Duration => Transmission.Duration;
    public string Reason => Transmission.Reason;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
    public IDggJson Json => new Websockets.SendableMute(Target.Nick, Duration);
    public string Twitch => $".timeout {Target} {Duration.TotalSeconds} {Reason}";
    public override string ToString() => Transmission.ToString();
  }
}
