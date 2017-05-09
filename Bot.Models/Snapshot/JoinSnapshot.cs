using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public class JoinSnapshot : Snapshot<IUser, Join> {
    public JoinSnapshot(IReceived<IUser, Join> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }

    public override IReadOnlyList<ISendable<ITransmittable>> Accept(ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> visitor) => visitor.Visit(this);
  }
}
