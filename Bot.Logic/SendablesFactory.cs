using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public abstract class SendablesFactory<TUser, TTransmission> : IErrorableFactory<ISnapshot<TUser, TTransmission>, IReadOnlyList<ISendable<ITransmittable>>>
    where TUser : IUser
    where TTransmission : ITransmittable {

    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new List<ISendable<ITransmittable>>();
    public abstract IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<TUser, TTransmission> input);
  }
}
