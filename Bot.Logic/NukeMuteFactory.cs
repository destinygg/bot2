using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class NukeMuteFactory : NukeAegisSendableFactoryBase, IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly ISettings _settings;
    private readonly IQueryCommandService<IUnitOfWork> _repository;

    public NukeMuteFactory(ISettings settings, ITimeService timeService, IQueryCommandService<IUnitOfWork> repository) : base(settings, timeService) {
      _settings = settings;
      _repository = repository;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(Nuke nuke, IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      _repository.Command(r => r.InMemory.Add(nuke));
      var msDelay = Math.Min(_settings.NukeMaximumLinger.TotalMilliseconds, nuke.Duration.TotalMilliseconds);
      Task.Delay(TimeSpan.FromMilliseconds(msDelay)).ContinueWith(_ => {
        _repository.Command(r => r.InMemory.Remove(nuke));
      });
      return GetCurrentVictims(nuke, context).Select(u => new SendableMute(u, nuke.Duration)).ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(NukeMuteFactory)}.").Wrap().ToList();
  }
}
