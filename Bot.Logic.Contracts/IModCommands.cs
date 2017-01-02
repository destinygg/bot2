using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IModCommands {
    ISendable Long(IReadOnlyList<IReceived> context);
    ISendable Sing();
  }
}
