using System;
using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("Muted {Target} for {Duration.TotalMinutes}m for: {Reason}")]
  public class SendableMute : Mute, ISendable {
    public SendableMute(Civilian target, TimeSpan duration) : base(target, duration) { }
    public SendableMute(Civilian target, TimeSpan duration, string reason) : base(target, duration, reason) { }
  }
}
