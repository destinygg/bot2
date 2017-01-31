using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Pipeline.Interfaces {
  public interface ISnapshotToSendable {
    IReadOnlyList<ISendable> GetSendables(ISnapshot<IUser, ITransmittable> snapshot);
  }
}
