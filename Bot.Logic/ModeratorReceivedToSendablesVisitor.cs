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

    public SendablesFactory Visit<TUser, TTransmission>(Received<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable =>
      SpecialVisit(received as dynamic);

    private SendablesFactory SpecialVisit(Received<Moderator, Message> message) => new SendablesFactory(snapshot =>
       _modCommandGenerator.Generate(snapshot).Concat(_commandGenerator.Generate(snapshot)).ToList());

    private SendablesFactory SpecialVisit<TUser, TTransmission>(Received<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable =>
      new SendablesFactory(_ => new List<ISendable<ITransmittable>>());
  }
}
