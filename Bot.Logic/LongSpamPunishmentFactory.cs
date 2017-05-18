using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {

  //todo: make the graduation more encompassing; it should start banning when people say 100 characters 50x for example
  //todo: remove duplicate spaces and other characters with http://stackoverflow.com/questions/4429995/how-do-you-remove-repeated-characters-in-a-string

  public class LongSpamPunishmentFactory : IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;
    private readonly ILogger _logger;

    public LongSpamPunishmentFactory(
      ITimeService timeService,
      ISettings settings,
      ILogger logger
    ) {
      _timeService = timeService;
      _settings = settings;
      _logger = logger;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
      var similarValues = snapshot.Context
        .OfType<IReceived<Civilian, PublicMessage>>()
        .Where(r => r.Timestamp + _settings.LongSpamWindow > _timeService.UtcNow)
        .Where(r => r.Transmission.Text.Length >= _settings.LongSpamMinimumLength)
        .Select(r => r.Transmission.Text.SimilarTo(snapshot.Latest.Transmission.Text)).ToList();
      var similarCount = similarValues.Count(p => p > _settings.MinimumPunishmentSimilarity);
      if (similarCount < 1) {
        return new List<ISendable<ITransmittable>>();
      } else {
        var banLength = TimeSpan.FromMinutes((snapshot.Latest.Transmission.Text.Length - _settings.LongSpamMinimumLength) / 10 + 1);
        return new SendableMute(snapshot.Sender(), banLength, $"{banLength.ToPretty(_logger)} {snapshot.Sender().Nick}: {similarValues.Average():#%} = past text").Wrap().ToList();
      }

    }

  }
}
