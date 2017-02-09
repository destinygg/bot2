using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class ModeratorSnapshotVisitor : BaseSnapshotVisitor<Moderator> {
    private readonly IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _modSendablesFactory;
    private readonly IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _commandGenerator;

    public ModeratorSnapshotVisitor(IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> modSendablesFactory, IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> commandGenerator, ILogger logger, ITimeService timeService) : base(logger) {
      _modSendablesFactory = modSendablesFactory;
      _commandGenerator = commandGenerator;
    }

    protected override IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<Moderator, PublicMessage> snapshot) =>
       ModAndCivilianCommands(snapshot);

    protected override IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<Moderator, PrivateMessage> snapshot) =>
       ModAndCivilianCommands(snapshot);

    private List<ISendable<ITransmittable>> ModAndCivilianCommands(ISnapshot<Moderator, Message> snapshot) =>
      _modSendablesFactory.Create(snapshot).Concat(_commandGenerator.Create(snapshot)).ToList();
  }
}
