using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Logic {
  public class NukeLogic : INukeLogic {
    private readonly IModCommandRegex _modCommandRegex;
    private readonly IReceivedFactory _receivedFactory;
    private readonly ITimeService _timeService;

    public NukeLogic(IModCommandRegex modCommandRegex, IReceivedFactory receivedFactory, ITimeService timeService) {
      _modCommandRegex = modCommandRegex;
      _receivedFactory = receivedFactory;
      _timeService = timeService;
    }

    public IReadOnlyList<ISendable> Nuke(IReceivedNuke nuke, IReadOnlyList<IReceived> context) =>
      _GetCurrentVictims(nuke, context)
      .Select(u => new SendableMute(u, nuke.Duration)).ToList();

    private IEnumerable<Civilian> _GetCurrentVictims(IReceivedNuke nuke, IEnumerable<IReceived> context) => context
      .OfType<MessageFromCivilian>()
      .Where(nuke.WillPunish)
      .Where(m => !_IsExpired(m, nuke))
      .Select(m => m.Sender)
      .OfType<Civilian>() // todo shadowing?
      .Distinct();

    private bool _IsExpired(ReceivedMessage message, IReceivedNuke nuke) {
      var punishmentTimestamp = message.Timestamp <= nuke.Timestamp ? nuke.Timestamp : message.Timestamp;
      var expirationDate = punishmentTimestamp + nuke.Duration;
      return expirationDate < _timeService.UtcNow;
    }

    public IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived> context) {
      var modMessages = context.OfType<MessageFromMod>().ToList();
      var nukes = _GetStringNukes(modMessages).Concat<IReceivedNuke>(_GetRegexNukes(modMessages));
      var victims = nukes.SelectMany(n => _GetCurrentVictims(n, context));

      //TODO consider checking if these are actual victims?
      var alreadyPardoned = context.OfType<ReceivedPardon>().Select(umb => umb.Target);
      return victims.Except(alreadyPardoned).Select(v => new SendablePardon(v)).ToList();
    }

    private IEnumerable<ReceivedStringNuke> _GetStringNukes(IEnumerable<ReceivedMessage> modMessages) => modMessages
      .Where(m => m.IsMatch(_modCommandRegex.Nuke))
      .Select(rm => _receivedFactory.ReceivedStringNuke(rm));

    private IEnumerable<ReceivedRegexNuke> _GetRegexNukes(IEnumerable<ReceivedMessage> modMessages) => modMessages
      .Where(m => m.IsMatch(_modCommandRegex.RegexNuke))
      .Select(rm => _receivedFactory.ReceivedRegexNuke(rm));
  }
}
