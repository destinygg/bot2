using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.ReceivedVisitor {
  public abstract class BaseReceivedVisitor<TUser> : IReceivedVisitor<DelegatedSnapshotFactory>
    where TUser : IUser {
    public abstract DelegatedSnapshotFactory Visit(IReceived<Civilian, PublicMessage> t);
    public abstract DelegatedSnapshotFactory Visit(IReceived<Moderator, PublicMessage> t);
    public abstract DelegatedSnapshotFactory Visit(IReceived<Moderator, ErrorMessage> t);
    public abstract DelegatedSnapshotFactory Visit(IReceived<Moderator, Pardon> t);
  }
}
