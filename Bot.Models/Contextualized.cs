using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class Contextualized<TUser, TTransmission> : IContextualized<TUser, TTransmission>
    where TTransmission : ITransmittable
    where TUser : IUser {

    public Contextualized(IReceived<TUser, TTransmission> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      Latest = latest;
      Context = context;
    }

    public IReceived<TUser, TTransmission> Latest { get; }
    public IReadOnlyList<IReceived<IUser, ITransmittable>> Context { get; }

  }
}
