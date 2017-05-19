using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IModCommandLogic {
    ISendable<PublicMessage> Long(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
    ISendable<PublicMessage> Sing();
    IReadOnlyList<ISendable<ITransmittable>> Nuke(IReadOnlyList<IReceived<IUser, ITransmittable>> context, Nuke nuke);
    IReadOnlyList<ISendable<ITransmittable>> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
    IReadOnlyList<ISendable<ITransmittable>> AddCommand(string command, string response);
    IReadOnlyList<ISendable<ITransmittable>> DelCommand(string command);
    IReadOnlyList<ISendable<ITransmittable>> Stalk(string command);
  }
}
