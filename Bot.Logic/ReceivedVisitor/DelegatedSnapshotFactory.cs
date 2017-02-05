using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools;

namespace Bot.Logic.ReceivedVisitor {
  public class DelegatedSnapshotFactory : DelegatedFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, ISnapshot<IUser, ITransmittable>> {
    public DelegatedSnapshotFactory(Func<IReadOnlyList<IReceived<IUser, ITransmittable>>, ISnapshot<IUser, ITransmittable>> create) : base(create) { }
  }
}
