using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public class PrivateMessageFromModSnapshot : PrivateMessageSnapshot<Moderator> {
    public PrivateMessageFromModSnapshot(IReceived<Moderator, PrivateMessage> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
    public override IReadOnlyList<ISendable<ITransmittable>> Accept(ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> visitor) => visitor.Visit(this);
  }
}
