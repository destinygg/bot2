using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic.SnapshotVisitor {
  public class SnapshotVisitor : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _modCommandFactory;
    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _banFactory;
    private readonly IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _commandFactory;
    private readonly ILogger _logger;

    public SnapshotVisitor(
      IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> modCommandFactory,
      IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> banFactory,
      IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> commandFactory,
      ILogger logger) {
      _modCommandFactory = modCommandFactory;
      _banFactory = banFactory;
      _commandFactory = commandFactory;
      _logger = logger;
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
      _logger.LogError(snapshot.Latest.Transmission.Text);
      return new List<ISendable<ITransmittable>>();
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, Pardon> snapshot) {
      _logger.LogError(snapshot.Latest.Transmission.ToString());
      return new List<ISendable<ITransmittable>>();
    }

  }
}
