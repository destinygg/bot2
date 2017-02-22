using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  class PublicMessageFromCivilianSnapshot : PublicMessageSnapshot<Civilian> {
    public PublicMessageFromCivilianSnapshot(IReceived<Civilian, PublicMessage> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
  }
}
