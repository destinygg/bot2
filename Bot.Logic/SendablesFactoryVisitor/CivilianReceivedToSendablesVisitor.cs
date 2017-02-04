using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SendablesFactoryVisitor {
  public class CivilianReceivedToSendablesVisitor : IReceivedVisitor<SendablesFactory> {
    private readonly IBanGenerator _banGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public CivilianReceivedToSendablesVisitor(IBanGenerator banGenerator, ICommandGenerator commandGenerator) {
      _banGenerator = banGenerator;
      _commandGenerator = commandGenerator;
    }

    public SendablesFactory Visit<TUser, TTransmission>(Received<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable =>
      SpecialVisit(received as dynamic);

    private SendablesFactory SpecialVisit(Received<Moderator, Message> message) => new SendablesFactory(snapshot => {
      var bans = _banGenerator.Generate(snapshot);
      return bans.Any()
        ? bans
        : _commandGenerator.Generate(snapshot);
    });

    private SendablesFactory SpecialVisit<TUser, TTransmission>(Received<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable =>
      new SendablesFactory(_ => new List<ISendable<ITransmittable>>());
  }
}
