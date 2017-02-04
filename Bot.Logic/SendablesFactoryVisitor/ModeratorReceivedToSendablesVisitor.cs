using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SendablesFactoryVisitor {
  public class ModeratorReceivedToSendablesVisitor : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IModCommandGenerator _modCommandGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public ModeratorReceivedToSendablesVisitor(IModCommandGenerator modCommandGenerator, ICommandGenerator commandGenerator) {
      _modCommandGenerator = modCommandGenerator;
      _commandGenerator = commandGenerator;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit<TUser, TTransmission>(ISnapshot<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable =>
      SpecialVisit(received as dynamic);

    private IReadOnlyList<ISendable<ITransmittable>> SpecialVisit(ISnapshot<Moderator, Message> snapshot) =>
       _modCommandGenerator.Generate(snapshot).Concat(_commandGenerator.Generate(snapshot)).ToList();

    private IReadOnlyList<ISendable<ITransmittable>> SpecialVisit(ISnapshot<IUser, ITransmittable> snapshot) =>
      new List<ISendable<ITransmittable>>();
  }
}
