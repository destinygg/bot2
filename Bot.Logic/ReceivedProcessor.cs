using System;
using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ReceivedProcessor : IReceivedProcessor {
    private readonly IMessageProcessor _messageProcessor;

    public ReceivedProcessor(IMessageProcessor messageProcessor) {
      _messageProcessor = messageProcessor;
    }

    public IEnumerable<ISendable> Process(IBanReceived banReceived) {
      throw new NotImplementedException();
    }

    public IEnumerable<ISendable> Process(IBroadcastReceived broadcastReceived) {
      throw new NotImplementedException();
    }

    public IEnumerable<ISendable> Process(ISubonlyReceived subonlyReceived) {
      throw new NotImplementedException();
    }

    public IEnumerable<ISendable> Process(IMuteReceived muteReceived) {
      throw new NotImplementedException();
    }

    public IEnumerable<ISendable> Process(IUnMuteBanReceived unMuteBanReceived) {
      throw new NotImplementedException();
    }

    public IEnumerable<ISendable> Process(IPublicMessageReceived publicMessageReceived) 
      => _messageProcessor.Process(publicMessageReceived);

    public IEnumerable<ISendable> Process(IPrivateMessageReceived privateMessageReceived)
      => _messageProcessor.Process(privateMessageReceived);
  }
}
