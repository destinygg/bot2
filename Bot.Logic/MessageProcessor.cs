using System;
using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class MessageProcessor : IMessageProcessor {
    private readonly IScanForBans _banScanner;
    private readonly IScanForModCommands _modCommandScanner;
    private readonly IScanForCommands _commandScanner;

    public MessageProcessor(IScanForBans banScanner, IScanForCommands commandScanner, IScanForModCommands modCommandScanner) {
      _banScanner = banScanner;
      _modCommandScanner = modCommandScanner;
      _commandScanner = commandScanner;
    }

    public IEnumerable<ISendable> Process(IPublicMessageReceived publicMessageReceived) {
      var outbox = new List<ISendable>();
      if (publicMessageReceived.Sender.IsMod) {
        outbox.AddRange(_modCommandScanner.Scan(publicMessageReceived));
      } else {
        outbox.AddRange(_banScanner.Scan(publicMessageReceived));
      }
      if (outbox.Count == 0) {
        outbox.AddRange(_commandScanner.Scan(publicMessageReceived));
      }
      return outbox;
    }

    public IEnumerable<ISendable> Process(IPrivateMessageReceived privateMessageReceived) {
      throw new NotImplementedException();
    }
  }
}
