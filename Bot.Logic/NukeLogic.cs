using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class NukeLogic : INukeLogic {
    private readonly IModCommandRegex _modCommandRegex;
    private readonly IReceivedFactory _receivedFactory;

    public NukeLogic(IModCommandRegex modCommandRegex, IReceivedFactory receivedFactory) {
      _modCommandRegex = modCommandRegex;
      _receivedFactory = receivedFactory;
    }

    public IReadOnlyList<ISendable> Nuke(IReceivedNuke nuke, IReadOnlyList<IReceived<IUser>> context) =>
      _GetCurrentVictims(nuke, context)
      .Select(u => new SendableMute(u, nuke.Duration)).ToList();

    private IEnumerable<Civilian> _GetCurrentVictims(IReceivedNuke nuke, IEnumerable<IReceived<IUser>> context) => context
      .OfType<IReceivedMessage<Civilian>>()
      .Where(nuke.WillPunish)
      .Select(m => m.Sender)
      .Distinct();

    public IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived<IUser>> context) {
      var modMessages = context.OfType<IReceivedMessage<Moderator>>().ToList();
      var nukes = _GetStringNukes(modMessages).Concat<IReceivedNuke>(_GetRegexNukes(modMessages));
      var victims = nukes.SelectMany(n => _GetCurrentVictims(n, context));

      //TODO consider checking if these are actual victims?
      var alreadyPardoned = context.OfType<ReceivedPardon>().Select(umb => umb.Target);
      return victims.Except(alreadyPardoned).Select(v => new SendablePardon(v)).ToList();
    }

    private IEnumerable<ReceivedStringNuke> _GetStringNukes(IEnumerable<IReceivedMessage<Moderator>> modMessages) => modMessages
      .Where(m => m.IsMatch(_modCommandRegex.Nuke))
      .Select(rm => _receivedFactory.ReceivedStringNuke(rm));

    private IEnumerable<ReceivedRegexNuke> _GetRegexNukes(IEnumerable<IReceivedMessage<Moderator>> modMessages) => modMessages
      .Where(m => m.IsMatch(_modCommandRegex.RegexNuke))
      .Select(rm => _receivedFactory.ReceivedRegexNuke(rm));
  }
}
