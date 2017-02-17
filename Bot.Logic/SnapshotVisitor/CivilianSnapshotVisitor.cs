using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic.SnapshotVisitor {
  public class CivilianSnapshotVisitor : BaseSnapshotVisitor<Civilian> {
    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _banFactory;
    private readonly IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _commandFactory;

    public CivilianSnapshotVisitor(IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> banFactory, IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> commandFactory, ILogger logger, ITimeService timeService) : base(logger) {
      _banFactory = banFactory;
      _commandFactory = commandFactory;
    }

    protected override IReadOnlyList<ISendable<ITransmittable>> _DynamicVisit(ISnapshot<Civilian, PublicMessage> snapshot) {
      var bans = _banFactory.Create(snapshot);
      return bans.Any()
        ? bans
        : _commandFactory.Create(snapshot);
    }

    protected override IReadOnlyList<ISendable<ITransmittable>> _DynamicVisit(ISnapshot<Civilian, PrivateMessage> snapshot) =>
      new List<ISendable<ITransmittable>>();
  }
}
