using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ReceivedProcessor : IReceivedProcessor {
    private readonly IContextualizedProcessor _contextualizedProcessor;

    public ReceivedProcessor(IContextualizedProcessor contextualizedProcessor) {
      _contextualizedProcessor = contextualizedProcessor;
    }

    public IEnumerable<ISendable> Process(IBanReceived banReceived)
      => _NoMessage;

    public IEnumerable<ISendable> Process(IBroadcastReceived broadcastReceived)
      => _NoMessage;

    public IEnumerable<ISendable> Process(ISubonlyReceived subonlyReceived)
      => _NoMessage;

    public IEnumerable<ISendable> Process(IMuteReceived muteReceived)
      => _NoMessage;

    public IEnumerable<ISendable> Process(IUnMuteBanReceived unMuteBanReceived)
      => _NoMessage;

    public IEnumerable<ISendable> Process(IPublicMessageReceived publicMessageReceived, IEnumerable<IPublicMessageReceived> context)
      => _contextualizedProcessor.Process(publicMessageReceived, context);

    public IEnumerable<ISendable> Process(IPrivateMessageReceived privateMessageReceived, IEnumerable<IPublicMessageReceived> context)
      => _contextualizedProcessor.Process(privateMessageReceived, context);

    private IEnumerable<ISendable> _NoMessage
      => new List<ISendable>();

  }
}
