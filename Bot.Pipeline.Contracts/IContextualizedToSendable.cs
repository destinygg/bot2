using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface ISnapshotToSendable {
    IReadOnlyList<ISendable> GetSendables(ISnapshot<IUser, ITransmittable> snapshot);
  }
}
