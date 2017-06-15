using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class CivilianPublicMessageToSendablesFactory : IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> {

    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _punishmentFactory;
    private readonly IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _commandFactory;
    private readonly IQueryCommandService<IUnitOfWork> _repository;
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;

    public CivilianPublicMessageToSendablesFactory(
      IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> punishmentFactory,
      IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> commandFactory,
      IQueryCommandService<IUnitOfWork> repository,
      ITimeService timeService,
      ISettings settings
      ) {
      _punishmentFactory = punishmentFactory;
      _commandFactory = commandFactory;
      _repository = repository;
      _timeService = timeService;
      _settings = settings;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
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

  }
}
