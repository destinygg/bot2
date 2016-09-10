using Bot.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Logic.Contracts {
  public interface IContextualizedProcessor {
    IEnumerable<ISendable> Process(IPublicMessageReceived publicMessageReceived, IEnumerable<IPublicMessageReceived> context);
    IEnumerable<ISendable> Process(IPrivateMessageReceived privateMessageReceived, IEnumerable<IPublicMessageReceived> context);
    IEnumerable<ISendable> Process(IContextualized contextualized);
  }
}
