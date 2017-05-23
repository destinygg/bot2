using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public abstract class PrivateMessageSnapshot<TUser> : MessageSnapshot<TUser, PrivateMessage>
    where TUser : IUser {
    protected PrivateMessageSnapshot(IReceived<TUser, PrivateMessage> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
  }
}
