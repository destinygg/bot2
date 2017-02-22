using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class SendableFactory : BaseSendableFactory<IUser, ITransmittable> {
    private readonly ILogger _logger;
    private readonly ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> _snapshotVisitor;

    public SendableFactory(ILogger logger, ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> snapshotVisitor) {
      _logger = logger;
      _snapshotVisitor = snapshotVisitor;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<IUser, ITransmittable> snapshot) {
      _logger.LogDebug(snapshot.Latest.ToString());
      return snapshot.Accept(_snapshotVisitor);
    }

  }
}
