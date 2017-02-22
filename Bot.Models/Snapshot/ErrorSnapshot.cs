using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Models.Snapshot {
  public class ErrorSnapshot : Snapshot<Moderator, ErrorMessage> {
    public ErrorSnapshot(IReceived<Moderator, ErrorMessage> latest, IReadOnlyList<IReceived<IUser, ITransmittable>> context) : base(latest, context) { }
  }
}
