using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class ModeratorSnapshotVisitor : BaseSnapshotVisitor<Moderator> {
    private readonly IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _modCommandFactory;
    private readonly IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _commandFactory;

    public ModeratorSnapshotVisitor(IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> modCommandFactory, IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> commandFactory, ILogger logger, ITimeService timeService) : base(logger) {
      _modCommandFactory = modCommandFactory;
      _commandFactory = commandFactory;
    }

    protected override IReadOnlyList<ISendable<ITransmittable>> _DynamicVisit(ISnapshot<Moderator, PublicMessage> snapshot) =>
       ModAndCivilianCommands(snapshot);

    protected override IReadOnlyList<ISendable<ITransmittable>> _DynamicVisit(ISnapshot<Moderator, PrivateMessage> snapshot) =>
       ModAndCivilianCommands(snapshot);

    private List<ISendable<ITransmittable>> ModAndCivilianCommands(ISnapshot<Moderator, Message> snapshot) =>
      _modCommandFactory.Create(snapshot).Concat(_commandFactory.Create(snapshot)).ToList();
  }
}
