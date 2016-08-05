using Bot.Models.Contracts;
using System.Collections.Generic;

namespace Bot.Logic.Contracts {
  public interface IMessageProcessor {
    IEnumerable<IBroadcast> Process(IPublicMessageReceived publicMessageReceived);
    IEnumerable<IBroadcast> Process(IPrivateMessageReceived privateMessageReceived);
  }
}
