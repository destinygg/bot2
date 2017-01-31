using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models {
  public class Snapshot<TUser, TTransmission> : ISnapshot<TUser, TTransmission>
    where TTransmission : ITransmittable
    where TUser : IUser {

    public Snapshot(IReceived<TUser, TTransmission> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      Latest = latest;
      Context = context;
    }

    public IReceived<TUser, TTransmission> Latest { get; }
    public IReadOnlyList<IReceived<IUser, ITransmittable>> Context { get; }

  }
}
