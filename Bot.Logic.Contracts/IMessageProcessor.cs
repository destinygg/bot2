using Bot.Models.Contracts;
using System.Collections.Generic;

namespace Bot.Logic.Contracts {
  public interface IMessageProcessor {
    IEnumerable<ISendable> Process(IPublicMessageReceived publicMessageReceived);
    IEnumerable<ISendable> Process(IPrivateMessageReceived privateMessageReceived);
  }
}
