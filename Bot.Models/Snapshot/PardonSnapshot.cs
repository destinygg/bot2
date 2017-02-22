using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public class PardonSnapshot : Snapshot<Moderator, Pardon> {
    public PardonSnapshot(IReceived<Moderator, Pardon> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
    public override IReadOnlyList<ISendable<ITransmittable>> Accept(ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> visitor) => visitor.Visit(this);
  }
}
