using System.Collections.Generic;

namespace Bot.Models.Contracts {
  public interface IContextualized<out TUser, out TTransmission>
    where TUser : IUser
    where TTransmission : ITransmittable{

    /// <summary> 
    /// The latest Received transmission
    /// </summary>
    IReceived<TUser, TTransmission> Latest { get; }

    /// <summary>
    /// The rest of the Received transmissions; doesn't include Latest
    /// </summary>
    IReadOnlyList<IReceived<IUser, ITransmittable>> Context { get; }
  }
}
