using System;
using Bot.Models.Interfaces;

namespace Bot.Models {
  public abstract class Received<TUser, TTransmission> : IReceived<TUser, TTransmission>
    where TUser : IUser
    where TTransmission : ITransmittable {
    protected Received(DateTime timestamp, TUser sender) {
      Timestamp = timestamp;
      Sender = sender;
    }

    public DateTime Timestamp { get; }
    public TUser Sender { get; }
    public abstract TTransmission Transmission { get; }
    public abstract TResult Accept<TResult>(IReceivedVisitor<TResult> visitor);
  }
}
