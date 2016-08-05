using System;
using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class MessageProcessor : IMessageProcessor {
    public IEnumerable<IBroadcast> Process(IPublicMessageReceived publicMessageReceived) {
      throw new NotImplementedException();
    }

    public IEnumerable<IBroadcast> Process(IPrivateMessageReceived privateMessageReceived) {
      throw new NotImplementedException();
    }
  }
}
