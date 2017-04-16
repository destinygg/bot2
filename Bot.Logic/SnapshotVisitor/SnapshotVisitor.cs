using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class SnapshotVisitor : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _modCommandFactory;
    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _banFactory;
    private readonly IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _commandFactory;

    public SnapshotVisitor(
      IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> modCommandFactory,
      IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> banFactory,
      IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> commandFactory) {
      _modCommandFactory = modCommandFactory;
      _banFactory = banFactory;
      _commandFactory = commandFactory;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Civilian, PublicMessage> snapshot) {
      var bans = _banFactory.Create(snapshot);
      return bans.Any()
        ? bans
        : _commandFactory.Create(snapshot);
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, PublicMessage> snapshot) =>
      _modCommandFactory.Create(snapshot).Concat(_commandFactory.Create(snapshot)).ToList();

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, ErrorMessage> snapshot) {
      throw new System.NotImplementedException();
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, Pardon> snapshot) {
      throw new System.NotImplementedException();
    }

  }
}
