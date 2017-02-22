using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public abstract class Snapshot<TUser, TTransmission> : ISnapshot<TUser, TTransmission>
    where TTransmission : ITransmittable
    where TUser : IUser {

    protected Snapshot(IReceived<TUser, TTransmission> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      Latest = latest;
      Context = context;
    }

    public IReceived<TUser, TTransmission> Latest { get; }
    public IReadOnlyList<IReceived<IUser, ITransmittable>> Context { get; }

    public abstract IReadOnlyList<ISendable<ITransmittable>> Accept(ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> visitor);
  }
}
