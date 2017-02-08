using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class SendableGenerator : SendablesFactory<IUser, ITransmittable> {
    private readonly ILogger _logger;
    private readonly IUserVisitor<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>> _userVisitor;

    public SendableGenerator(ILogger logger, IUserVisitor<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>> userVisitor) {
      _logger = logger;
      _userVisitor = userVisitor;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<IUser, ITransmittable> snapshot) {
      _logger.LogVerbose(snapshot.Latest.ToString());
      var snapshotVisitor = snapshot.Latest.Sender.Accept(_userVisitor);
      return snapshot.Accept(snapshotVisitor);
    }

  }
}
