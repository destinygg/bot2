using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public abstract class NukeAegisBase {
    private readonly ISettings _settings;
    private readonly ITimeService _timeService;

    protected NukeAegisBase(ISettings settings, ITimeService timeService) {
      _settings = settings;
      _timeService = timeService;
    }

    protected IEnumerable<Civilian> GetCurrentVictims(ParsedNuke nuke, IEnumerable<IReceived<IUser, ITransmittable>> context) => context
      .OfType<IReceived<Civilian, PublicMessage>>()
      .Where(m => WillPunish(nuke, m))
      .Select(m => m.Sender)
      .Distinct();

    private bool WillPunish(ParsedNuke nuke, IReceived<Civilian, PublicMessage> message) =>
      nuke.MatchesNukedTerm(message.Transmission.Text) &&
      _WithinRange(nuke, message) &&
      !_IsExpired(nuke, message);

    private bool _WithinRange(ParsedNuke nuke, IReceived<Civilian, PublicMessage> message) =>
      message.Timestamp.IsWithin(nuke.Timestamp, _settings.NukeBlastRadius);

    private bool _IsExpired(ParsedNuke nuke, IReceived<Civilian, PublicMessage> message) {
      var punishmentTimestamp = message.Timestamp <= nuke.Timestamp ? nuke.Timestamp : message.Timestamp;
      var expirationDate = punishmentTimestamp + nuke.Duration;
      return expirationDate < _timeService.UtcNow;
    }

  }
}
