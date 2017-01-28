using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public interface IModCommandLogic {
    ISendable Long(IReadOnlyList<IReceived<IUser>> context);
    ISendable Sing();
    IReadOnlyList<ISendable> Nuke(IReadOnlyList<IReceived<IUser>> context, IReceivedNuke nuke);
    IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived<IUser>> context);
  }
}
