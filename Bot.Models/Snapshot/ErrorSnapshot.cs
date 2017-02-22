using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public class ErrorSnapshot : Snapshot<Moderator, ErrorMessage> {
    public ErrorSnapshot(IReceived<Moderator, ErrorMessage> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
    public override IReadOnlyList<ISendable<ITransmittable>> Accept(ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> visitor) => visitor.Visit(this);
  }
}
