using System;

namespace Bot.Models.Contracts {
  public interface IReceived<out TUser, out TTransmission>
    where TUser : IUser
    where TTransmission : ITransmittable {
    DateTime Timestamp { get; }
    TUser Sender { get; }
    TTransmission Transmission { get; }

  }
}
