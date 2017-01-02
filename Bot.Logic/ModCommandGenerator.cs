using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;
using Bot.Tools;

namespace Bot.Logic {

  public class ModCommandGenerator : IModCommandGenerator {
    private readonly IModCommands _modCommands;

    public ModCommandGenerator(IModCommands modCommands) {
      _modCommands = modCommands;
    }

    public IReadOnlyList<ISendable> Generate(IContextualized contextualized) {
      var context = contextualized.Context;
      var message = contextualized.First as IPublicMessageReceived;
      if (message != null) {
        if (message.StartsWith("!sing"))
          return _modCommands.Sing().Wrap().ToList();
        if (message.StartsWith("!long"))
          return _modCommands.Long(context).Wrap().ToList();
      }
      return new List<ISendable>();
    }

  }
}
