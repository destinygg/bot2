using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class SendableMute : Mute {
    public SendableMute(IUser target, TimeSpan duration) : base(target, duration) { }
    public SendableMute(IUser target, TimeSpan duration, string reason) : base(target, duration, reason) { }

  }
}
