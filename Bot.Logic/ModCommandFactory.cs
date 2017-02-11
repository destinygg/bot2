﻿using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools;

namespace Bot.Logic {
  public class ModCommandFactory : BaseSendableFactory<Moderator, IMessage> {
    private readonly IModCommandLogic _modCommandLogic;
    private readonly IModCommandParser _modCommandParser;
    private readonly IModCommandRegex _modCommandRegex;
    private readonly IReceivedFactory _receivedFactory;

    public ModCommandFactory(IModCommandLogic modCommandLogic, IModCommandParser modCommandParser, IModCommandRegex modCommandRegex, IReceivedFactory receivedFactory) {
      _modCommandLogic = modCommandLogic;
      _modCommandParser = modCommandParser;
      _modCommandRegex = modCommandRegex;
      _receivedFactory = receivedFactory;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Moderator, IMessage> snapshot) {
      var context = snapshot.Context;
      var message = snapshot.Latest;
      if (message.IsMatch(_modCommandRegex.Sing))
        return _modCommandLogic.Sing().Wrap().ToList();
      if (message.StartsWith("!long"))
        return _modCommandLogic.Long(context).Wrap().ToList();
      if (message.IsMatch(_modCommandRegex.Nuke))
        return _modCommandLogic.Nuke(context, _receivedFactory.ParsedNuke(message));
      if (message.IsMatch(_modCommandRegex.RegexNuke))
        return _modCommandLogic.Nuke(context, _receivedFactory.ParsedNuke(message));
      if (message.IsMatch(_modCommandRegex.Aegis))
        return _modCommandLogic.Aegis(context);
      return new List<ISendable<ITransmittable>>();
    }

  }
}