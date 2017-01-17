using System;
using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface INukeLogic {
    IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived> context);
    IReadOnlyList<ISendable> Nuke(IReadOnlyList<IReceived> context, string phrase, TimeSpan duration);
    IReadOnlyList<ISendable> RegexNuke(IReadOnlyList<IReceived> context, string phrase, TimeSpan duration);
  }
}