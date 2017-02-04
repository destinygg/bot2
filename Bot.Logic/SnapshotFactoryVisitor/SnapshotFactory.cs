using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools;

namespace Bot.Logic.SnapshotFactoryVisitor {
  public class SnapshotFactory : DelegatedFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, ISnapshot<IUser, ITransmittable>> {
    public SnapshotFactory(Func<IReadOnlyList<IReceived<IUser, ITransmittable>>, ISnapshot<IUser, ITransmittable>> create) : base(create) { }
  }
}