using System;
using System.Collections.Generic;

namespace Bot.Models.Interfaces {
  public interface IReceivedVisitor<out TResult> {
    TResult Visit(ReceivedPardon pardon);
    TResult Visit<TUser>(ReceivedPublicMessage<TUser> receivedPublicMessage) where TUser : IUser;
  }
}
