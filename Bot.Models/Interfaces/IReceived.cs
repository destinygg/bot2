using System;
using System.Collections.Generic;

namespace Bot.Models.Interfaces {
  public interface IReceived<out TUser, out TTransmission>
    where TUser : IUser
    where TTransmission : ITransmittable {
    DateTime Timestamp { get; }
    TUser Sender { get; }
    TTransmission Transmission { get; }
    Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> Accept(IReceivedVisitor visitor);
  }
}
