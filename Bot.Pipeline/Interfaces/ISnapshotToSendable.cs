using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Interfaces {
  public interface ISnapshotToSendable {
    IReadOnlyList<ISendable> GetSendables(ISnapshot<IUser, ITransmittable> snapshot);
  }
}
