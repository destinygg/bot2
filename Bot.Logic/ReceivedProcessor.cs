using System;
using System.Collections.Generic;
using Bot.Client.Contracts;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ReceivedProcessor : IReceivedProcessor {
    private readonly IMessageProcessor _messageProcessor;

    public ReceivedProcessor(IMessageProcessor messageProcessor) {
      _messageProcessor = messageProcessor;
    }

    IEnumerable<ISendable> IReceivedProcessor.Process(IBanReceived banReceived) {
      throw new NotImplementedException();
    }

    IEnumerable<ISendable> IReceivedProcessor.Process(IBroadcastReceived broadcastReceived) {
      throw new NotImplementedException();
    }

    IEnumerable<ISendable> IReceivedProcessor.Process(ISubonlyReceived subonlyReceived) {
      throw new NotImplementedException();
    }

    IEnumerable<ISendable> IReceivedProcessor.Process(IMuteReceived muteReceived) {
      throw new NotImplementedException();
    }

    IEnumerable<ISendable> IReceivedProcessor.Process(IUnMuteBanReceived unMuteBanReceived) {
      throw new NotImplementedException();
    }

    IEnumerable<ISendable> IReceivedProcessor.Process(IPublicMessageReceived publicMessageReceived) 
      => _messageProcessor.Process(publicMessageReceived);

    IEnumerable<ISendable> IReceivedProcessor.Process(IPrivateMessageReceived privateMessageReceived)
      => _messageProcessor.Process(privateMessageReceived);
  }
}
