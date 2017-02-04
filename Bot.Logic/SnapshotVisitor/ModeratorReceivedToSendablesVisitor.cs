using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class ModeratorReceivedToSendablesVisitor : FromUserToSendablesVisitor<Moderator> {
    private readonly IModCommandGenerator _modCommandGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public ModeratorReceivedToSendablesVisitor(IModCommandGenerator modCommandGenerator, ICommandGenerator commandGenerator, ILogger logger, ITimeService timeService) : base(logger) {
      _modCommandGenerator = modCommandGenerator;
      _commandGenerator = commandGenerator;
    }

    protected override IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<Moderator, PublicMessage> snapshot) =>
       ModAndCivilianCommands(snapshot);

    protected override IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<Moderator, PrivateMessage> snapshot) =>
       ModAndCivilianCommands(snapshot);

    private List<ISendable<ITransmittable>> ModAndCivilianCommands(ISnapshot<Moderator, Message> snapshot) =>
      _modCommandGenerator.Generate(snapshot).Concat(_commandGenerator.Generate(snapshot)).ToList();
  }
}
