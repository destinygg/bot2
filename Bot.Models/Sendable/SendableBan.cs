﻿using System;
using Bot.Models.Interfaces;

namespace Bot.Models.Sendable {
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
    public object Json => new Websockets.SendableBan(Target.Nick, false, Duration, false, Reason);
    public override string ToString() => $"Banned {Target} for {Duration.TotalMinutes}m for: {Reason}";
  }
}
