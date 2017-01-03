using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IModCommandLogic {
    ISendable Long(IReadOnlyList<IReceived> context);
    ISendable Sing();
  }
}
