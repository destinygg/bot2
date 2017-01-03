using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;
using Bot.Tools;

namespace Bot.Logic {

  public class ModCommandGenerator : IModCommandGenerator {
    private readonly IModCommandLogic _modCommandLogic;

    public ModCommandGenerator(IModCommandLogic modCommandLogic) {
      _modCommandLogic = modCommandLogic;
    }

    public IReadOnlyList<ISendable> Generate(IContextualized contextualized) {
      var context = contextualized.Context;
      var message = contextualized.First as IPublicMessageReceived;
      if (message != null) {
        if (message.StartsWith("!sing"))
          return _modCommandLogic.Sing().Wrap().ToList();
        if (message.StartsWith("!long"))
          return _modCommandLogic.Long(context).Wrap().ToList();
      }
      return new List<ISendable>();
    }

  }
}
