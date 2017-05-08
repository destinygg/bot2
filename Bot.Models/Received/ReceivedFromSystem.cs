using System;
using Bot.Models.Interfaces;

namespace Bot.Models.Received {
  public abstract class ReceivedFromSystem<TTransmission> : IReceived<Moderator, TTransmission>
    where TTransmission : ITransmittable {
    protected ReceivedFromSystem(DateTime timestamp) {
      Timestamp = timestamp;
    }

    public DateTime Timestamp { get; }
    public Moderator Sender => new Moderator("FROM_CLIENT_SYSTEM");
    public abstract TTransmission Transmission { get; }
    public abstract TResult Accept<TResult>(IReceivedVisitor<TResult> visitor);
  }
}
