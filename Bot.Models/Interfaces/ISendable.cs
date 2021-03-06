﻿using Bot.Models.Websockets;

namespace Bot.Models.Interfaces {
  public interface ISendable<out T>
    where T : ITransmittable {
    T Transmission { get; }
    TResult Accept<TResult>(ISendableVisitor<TResult> visitor);
    IDggJson Json { get; }
    string Twitch { get; }
  }
}
