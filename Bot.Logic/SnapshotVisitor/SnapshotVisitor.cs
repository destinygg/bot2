using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic.SnapshotVisitor {
  public class SnapshotVisitor : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _modCommandFactory;
    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _punishmentFactory;
    private readonly IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _commandFactory;
    private readonly IQueryCommandService<IUnitOfWork> _repository;
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;
    private readonly ILogger _logger;

    public SnapshotVisitor(
      IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> modCommandFactory,
      IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> punishmentFactory,
      IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> commandFactory,
      IQueryCommandService<IUnitOfWork> repository,
      ITimeService timeService,
      ISettings settings,
      ILogger logger) {
      _modCommandFactory = modCommandFactory;
      _punishmentFactory = punishmentFactory;
      _commandFactory = commandFactory;
      _repository = repository;
      _timeService = timeService;
      _settings = settings;
      _logger = logger;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Civilian, PublicMessage> snapshot) {
      var bans = _punishmentFactory.Create(snapshot);
      if (bans.Any()) {
        return bans;
      }
      var latestCivilianCommandTime = _repository.Query(u => u.InMemory.LatestCivilianCommandTime);
      if (_timeService.UtcNow > latestCivilianCommandTime.Add(_settings.CivilianCommandInterval)) {
        var commands = _commandFactory.Create(snapshot);
        if (commands.Any())
          _repository.Command(u => u.InMemory.LatestCivilianCommandTime = _timeService.UtcNow);
        return commands;
      }
      return new List<ISendable<ITransmittable>>();
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

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, InitialUsers> initialUsers) => new List<ISendable<ITransmittable>>();

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<IUser, Join> join) => new List<ISendable<ITransmittable>>();
    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<IUser, Quit> join) => new List<ISendable<ITransmittable>>();
  }
}
