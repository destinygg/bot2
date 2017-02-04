using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SendablesFactoryVisitor {
  public class CivilianReceivedToSendablesVisitor : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IBanGenerator _banGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public CivilianReceivedToSendablesVisitor(IBanGenerator banGenerator, ICommandGenerator commandGenerator) {
      _banGenerator = banGenerator;
      _commandGenerator = commandGenerator;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit<TUser, TTransmission>(ISnapshot<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable =>
      SpecialVisit(received as dynamic);

    private IReadOnlyList<ISendable<ITransmittable>> SpecialVisit(ISnapshot<Civilian, PublicMessage> snapshot) {
      var bans = _banGenerator.Generate(snapshot);
      return bans.Any()
        ? bans
        : _commandGenerator.Generate(snapshot);
    }

    private IReadOnlyList<ISendable<ITransmittable>> SpecialVisit(ISnapshot<IUser, ITransmittable> snapshot) =>
      new List<ISendable<ITransmittable>>();
  }
}
