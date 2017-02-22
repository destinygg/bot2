using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public abstract class PublicMessageSnapshot<TUser> : MessageSnapshot<TUser, PublicMessage>
    where TUser : IUser {
    protected PublicMessageSnapshot(IReceived<TUser, PublicMessage> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
  }
}
