using System;
using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IModCommandLogic {
    ISendable Long(IReadOnlyList<IReceived> context);
    ISendable Sing();
    IReadOnlyList<ISendable> Nuke(IReadOnlyList<IReceived> context, string phrase, TimeSpan duration);
    IReadOnlyList<ISendable> RegexNuke(IReadOnlyList<IReceived> context, string phrase, TimeSpan duration);
    IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived> context);
  }
}
