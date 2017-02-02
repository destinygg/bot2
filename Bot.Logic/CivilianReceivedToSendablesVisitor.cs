using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class CivilianReceivedToSendablesVisitor : IReceivedVisitor<SendablesFactory> {
    private readonly IBanGenerator _banGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public CivilianReceivedToSendablesVisitor(IBanGenerator banGenerator, ICommandGenerator commandGenerator) {
      _banGenerator = banGenerator;
      _commandGenerator = commandGenerator;
    }

    public SendablesFactory Visit(ReceivedPardon pardon) => new SendablesFactory(_ => new List<ISendable<ITransmittable>>());

    public SendablesFactory Visit<TUser>(ReceivedPublicMessage<TUser> receivedPublicMessage) where TUser : IUser => new SendablesFactory(snapshot => {
      var bans = _banGenerator.Generate(snapshot);
      return bans.Any()
        ? bans
        : _commandGenerator.Generate(snapshot);
    });

  }
}
