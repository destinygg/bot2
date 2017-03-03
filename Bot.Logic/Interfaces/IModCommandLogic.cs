using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IModCommandLogic {
    ISendable<PublicMessage> Long(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
    ISendable<PublicMessage> Sing();
    IReadOnlyList<ISendable<ITransmittable>> Nuke(IReadOnlyList<IReceived<IUser, ITransmittable>> context, IParsedNuke nuke);
    IReadOnlyList<ISendable<Pardon>> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
  }
}
