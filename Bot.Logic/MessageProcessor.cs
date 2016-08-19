using System.Collections.Generic;
using System.Threading.Tasks;
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

    private IEnumerable<ISendable> _Process(IPublicMessageReceived publicMessageReceived, IEnumerable<IPublicMessageReceived> context) {
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

    private IEnumerable<ISendable> _Process(IPrivateMessageReceived privateMessageReceived, IEnumerable<IPublicMessageReceived> context) {
      var outbox = new List<ISendable>();
      if (privateMessageReceived.Sender.IsMod)
        outbox.AddRange(_modCommandScanner.Scan(privateMessageReceived, context));
      return outbox;
    }

    public async Task<IEnumerable<ISendable>> Process(IPublicMessageReceived publicMessageReceived, IEnumerable<IPublicMessageReceived> context) {
      return await Task.Run(() => _Process(publicMessageReceived, context));
    }

    public async Task<IEnumerable<ISendable>> Process(IPrivateMessageReceived privateMessageReceived, IEnumerable<IPublicMessageReceived> context) {
      return await Task.Run(() => _Process(privateMessageReceived, context));
    }
  }
}
