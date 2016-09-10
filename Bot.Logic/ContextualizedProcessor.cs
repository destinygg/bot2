using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ContextualizedProcessor : IContextualizedProcessor {
    private readonly IScanForBans _banScanner;
    private readonly IScanForModCommands _modCommandScanner;
    private readonly IScanForCommands _commandScanner;

    public ContextualizedProcessor(IScanForBans banScanner, IScanForCommands commandScanner, IScanForModCommands modCommandScanner) {
      _banScanner = banScanner;
      _modCommandScanner = modCommandScanner;
      _commandScanner = commandScanner;
    }

    public IEnumerable<ISendable> Process(IPublicMessageReceived publicMessageReceived, IEnumerable<IPublicMessageReceived> context) {
      var outbox = new List<ISendable>();
      if (publicMessageReceived.Sender.IsMod) {
        outbox.AddRange(_modCommandScanner.Scan(publicMessageReceived, context));
      } else {
        outbox.AddRange(_banScanner.Scan(publicMessageReceived));
      }
      if (outbox.Count == 0) {
        outbox.AddRange(_commandScanner.Scan(publicMessageReceived));
      }
      return outbox;
    }

    public IEnumerable<ISendable> Process(IContextualized contextualized) {
      var context = contextualized.Context.Where(c => c is IPublicMessageReceived).Cast<IPublicMessageReceived>();
      if (contextualized.First is IPublicMessageReceived) {
        return Process((IPublicMessageReceived) contextualized.First, context);
      }
      return new List<ISendable>();
    }

    public IEnumerable<ISendable> Process(IPrivateMessageReceived privateMessageReceived, IEnumerable<IPublicMessageReceived> context) {
      var outbox = new List<ISendable>();
      if (privateMessageReceived.Sender.IsMod)
        outbox.AddRange(_modCommandScanner.Scan(privateMessageReceived, context));
      return outbox;
    }


  }
}
