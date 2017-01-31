using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IModCommandLogic {
    ISendable Long(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
    ISendable Sing();
    IReadOnlyList<ISendable> Nuke(IReadOnlyList<IReceived<IUser, ITransmittable>> context, IParsedNuke nuke);
    IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
  }
}
