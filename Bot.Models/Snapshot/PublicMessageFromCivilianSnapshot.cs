using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public class PublicMessageFromCivilianSnapshot : PublicMessageSnapshot<Civilian> {
    public PublicMessageFromCivilianSnapshot(IReceived<Civilian, PublicMessage> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
    public override IReadOnlyList<ISendable<ITransmittable>> Accept(ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> visitor) => visitor.Visit(this);
  }
}
