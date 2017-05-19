using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class SelfSpamPunishmentFactory : IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;

    public SelfSpamPunishmentFactory(ITimeService timeService, ISettings settings) {
      _timeService = timeService;
      _settings = settings;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
      var similarValues = snapshot.Context
        .OfType<IReceived<Civilian, PublicMessage>>()
        .Where(r => r.Timestamp + _settings.SelfSpamWindow > _timeService.UtcNow)
        .Where(r => r.Sender.Nick == snapshot.Latest.Sender.Nick)
        .Select(r => r.Transmission.Text.SimilarTo(snapshot.Latest.Transmission.Text)).ToList();
      var similarCount = similarValues.Count(p => p > _settings.MinimumPunishmentSimilarity);
      return similarCount < 2
        ? (IReadOnlyList<ISendable<ITransmittable>>) new List<ISendable<ITransmittable>>()
        : new SendableMute(snapshot.Sender(), TimeSpan.FromMinutes(2), $"2m {snapshot.Sender().Nick}: {similarValues.Max():#%} = your past text").Wrap().ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new List<ISendable<ITransmittable>>();
  }
}
