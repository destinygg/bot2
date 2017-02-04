using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class CivilianReceivedToSendablesVisitor : FromUserToSendablesVisitor<Civilian> {
    private readonly IBanGenerator _banGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public CivilianReceivedToSendablesVisitor(IBanGenerator banGenerator, ICommandGenerator commandGenerator, ILogger logger, ITimeService timeService) : base(logger) {
      _banGenerator = banGenerator;
      _commandGenerator = commandGenerator;
    }

    protected override IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<Civilian, PublicMessage> snapshot) {
      var bans = _banGenerator.Generate(snapshot);
      return bans.Any()
        ? bans
        : _commandGenerator.Generate(snapshot);
    }

    protected override IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<Civilian, PrivateMessage> snapshot) =>
      new List<ISendable<ITransmittable>>();
  }
}
