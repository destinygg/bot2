using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class NukeMuteFactory : NukeAegisSendableFactoryBase, IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IQueryCommandService<IUnitOfWork> _repository;

    public NukeMuteFactory(ISettings settings, ITimeService timeService, IQueryCommandService<IUnitOfWork> repository) : base(settings, timeService) {
      _repository = repository;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(Nuke nuke, IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      _repository.Command(r => r.InMemory.Add(nuke));
      return GetCurrentVictims(nuke, context).Select(u => new SendableMute(u, nuke.Duration)).ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(NukeMuteFactory)}.").Wrap().ToList();
  }
}
