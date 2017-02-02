using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools;

namespace Bot.Logic {
  public class SendablesFactory : DelegatedFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> {
    public SendablesFactory(Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> create) : base(create) { }
  }
}
