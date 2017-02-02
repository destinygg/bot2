using System;
using System.Collections.Generic;

namespace Bot.Models.Interfaces {
  public interface IReceivedVisitor {
    Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> Visit(ReceivedPardon pardon);
    Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> Visit<TUser>(ReceivedPublicMessage<TUser> receivedPublicMessage) where TUser : IUser;
  }
}
