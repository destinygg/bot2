using Bot.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Logic.Contracts {
  public interface IMessageProcessor {
    Task<IEnumerable<ISendable>> Process(IPublicMessageReceived publicMessageReceived, IEnumerable<IPublicMessageReceived> context);
    Task<IEnumerable<ISendable>> Process(IPrivateMessageReceived privateMessageReceived, IEnumerable<IPublicMessageReceived> context);
  }
}
