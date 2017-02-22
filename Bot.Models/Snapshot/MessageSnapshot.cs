using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public abstract class MessageSnapshot<TUser, TTransmission> : Snapshot<TUser, TTransmission>
    where TTransmission : ITransmittable
    where TUser : IUser {
    protected MessageSnapshot(IReceived<TUser, TTransmission> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
  }
}
