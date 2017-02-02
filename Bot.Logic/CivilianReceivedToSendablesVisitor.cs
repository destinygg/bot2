using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class CivilianReceivedToSendablesVisitor : IReceivedVisitor<Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>> {
    private readonly IBanGenerator _banGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public CivilianReceivedToSendablesVisitor(IBanGenerator banGenerator, ICommandGenerator commandGenerator) {
      _banGenerator = banGenerator;
      _commandGenerator = commandGenerator;
    }

    public Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> Visit(ReceivedPardon pardon) =>
      _ => new List<ISendable<ITransmittable>>();

    public Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> Visit<TUser>(ReceivedPublicMessage<TUser> receivedPublicMessage) where TUser : IUser => snapshot => {
      var bans = _banGenerator.Generate(snapshot);
      return bans.Any()
        ? bans
        : _commandGenerator.Generate(snapshot);
    };

  }
}
