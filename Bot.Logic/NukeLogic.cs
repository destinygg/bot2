﻿using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class NukeLogic : INukeLogic {
    private readonly IModCommandRegex _modCommandRegex;
    private readonly IReceivedFactory _receivedFactory;

    public NukeLogic(IModCommandRegex modCommandRegex, IReceivedFactory receivedFactory) {
      _modCommandRegex = modCommandRegex;
      _receivedFactory = receivedFactory;
    }

    public IReadOnlyList<ISendable<Mute>> Nuke(IParsedNuke nuke, IReadOnlyList<IReceived<IUser, ITransmittable>> context) =>
      _GetCurrentVictims(nuke, context)
      .Select(u => new SendableMute(u, nuke.Duration)).ToList();

    private IEnumerable<Civilian> _GetCurrentVictims(IParsedNuke nuke, IEnumerable<IReceived<IUser, ITransmittable>> context) => context
      .OfType<IReceived<Civilian, PublicMessage>>()
      .Where(nuke.WillPunish)
      .Select(m => m.Sender)
      .Distinct();

    public IReadOnlyList<ISendable<Pardon>> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      var modMessages = context.OfType<IReceived<Moderator, IMessage>>().ToList();
      var nukes = _GetStringNukes(modMessages).Concat<IParsedNuke>(_GetRegexNukes(modMessages));
      var victims = nukes.SelectMany(n => _GetCurrentVictims(n, context));

      //TODO consider checking if these are actual victims?
      var alreadyPardoned = context.OfType<ReceivedPardon>().Select(umb => umb.Target);
      return victims.Except(alreadyPardoned).Select(v => new SendablePardon(v)).ToList();
    }

    private IEnumerable<ParsedNuke> _GetStringNukes(IEnumerable<IReceived<Moderator, IMessage>> modMessages) => modMessages
      .Where(m => m.IsMatch(_modCommandRegex.Nuke))
      .Select(rm => _receivedFactory.ParsedNuke(rm));

    private IEnumerable<ParsedNuke> _GetRegexNukes(IEnumerable<IReceived<Moderator, IMessage>> modMessages) => modMessages
      .Where(m => m.IsMatch(_modCommandRegex.RegexNuke))
      .Select(rm => _receivedFactory.ParsedNuke(rm));
  }
}
