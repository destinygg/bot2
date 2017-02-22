using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class SnapshotVisitor : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> {

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Civilian, PublicMessage> t) {
      throw new System.NotImplementedException();
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, PublicMessage> t) {
      throw new System.NotImplementedException();
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, ErrorMessage> t) {
      throw new System.NotImplementedException();
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, Pardon> t) {
      throw new System.NotImplementedException();
    }

  }
}
