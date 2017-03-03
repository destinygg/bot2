using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public abstract class BaseSendableFactory<TUser, TTransmission> : IErrorableFactory<ISnapshot<TUser, TTransmission>, IReadOnlyList<ISendable<ITransmittable>>>
    where TUser : IUser
    where TTransmission : ITransmittable {

    public abstract IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<TUser, TTransmission> input);
    public abstract IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate { get; }
  }
}
