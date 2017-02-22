using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public class PublicMessageFromModSnapshot : PublicMessageSnapshot<Moderator> {
    public PublicMessageFromModSnapshot(IReceived<Moderator, PublicMessage> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
  }
}
