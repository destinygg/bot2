using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class SendableMute : Mute, ISendable {
    public SendableMute(IUser target, TimeSpan duration) : base(target, duration) { }
    public SendableMute(IUser target, TimeSpan duration, string reason) : base(target, duration, reason) { }
    public override string ToString() => $"Sending a Mute targeting {Target.Nick} for {Duration.TotalMinutes}m with the reason: {Reason}";
  }
}
