using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.ReceivedVisitor {
  public abstract class BaseReceivedVisitor<TUser> : IReceivedVisitor<DelegatedSnapshotFactory>
    where TUser : IUser {

    protected BaseReceivedVisitor() { }

    public abstract DelegatedSnapshotFactory Visit<TVisitedUser, TTransmission>(
        Received<TVisitedUser, TTransmission> received)
      // todo: TVisitedUser should be the same as TUser
      where TVisitedUser : IUser
      where TTransmission : ITransmittable;
  }
}
