using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class ModeratorReceivedToSendablesVisitor : IReceivedVisitor<SendablesFactory> {
    private readonly IModCommandGenerator _modCommandGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public ModeratorReceivedToSendablesVisitor(IModCommandGenerator modCommandGenerator, ICommandGenerator commandGenerator) {
      _modCommandGenerator = modCommandGenerator;
      _commandGenerator = commandGenerator;
    }

    public SendablesFactory Visit(ReceivedPardon pardon) => new SendablesFactory(_ => new List<ISendable<ITransmittable>>());

    public SendablesFactory Visit<TUser>(ReceivedPublicMessage<TUser> receivedPublicMessage) where TUser : IUser => new SendablesFactory(snapshot => 
      _modCommandGenerator.Generate(snapshot).Concat(_commandGenerator.Generate(snapshot)).ToList());
  }
}
