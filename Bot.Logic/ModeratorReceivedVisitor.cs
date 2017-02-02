using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class ModeratorReceivedVisitor : IReceivedVisitor {
    private readonly IModCommandGenerator _modCommandGenerator;
    private readonly ICommandGenerator _commandGenerator;

    public ModeratorReceivedVisitor(IModCommandGenerator modCommandGenerator, ICommandGenerator commandGenerator) {
      _modCommandGenerator = modCommandGenerator;
      _commandGenerator = commandGenerator;
    }

    public Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> Visit(ReceivedPardon pardon) =>
      _ => new List<ISendable<ITransmittable>>();

    public Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> Visit<TUser>(ReceivedPublicMessage<TUser> receivedPublicMessage) where TUser : IUser => snapshot =>
      _modCommandGenerator.Generate(snapshot).Concat(_commandGenerator.Generate(snapshot)).ToList();

  }
}
