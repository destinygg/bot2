using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IReceivedProcessor {
    Task<IEnumerable<ISendable>> Process(IPrivateMessageReceived privateMessageReceived);
    Task<IEnumerable<ISendable>> Process(IPublicMessageReceived publicMessageReceived, IEnumerable<IPublicMessageReceived> context);
    Task<IEnumerable<ISendable>> Process(IMuteReceived muteReceived);
    Task<IEnumerable<ISendable>> Process(IUnMuteBanReceived unMuteBanReceived);
    Task<IEnumerable<ISendable>> Process(ISubonlyReceived subonlyReceived);
    Task<IEnumerable<ISendable>> Process(IBanReceived banReceived);
    Task<IEnumerable<ISendable>> Process(IBroadcastReceived broadcastReceived);
  }
}
