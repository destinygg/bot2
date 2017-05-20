using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IModCommandRepositoryLogic {
    IReadOnlyList<ISendable<ITransmittable>> AddCommand(string command, string response);
    IReadOnlyList<ISendable<ITransmittable>> DelCommand(string command);
  }
}
