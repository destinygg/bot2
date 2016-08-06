using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IReceivedProcessor {
    IEnumerable<ISendable> Process(IPrivateMessageReceived privateMessageReceived);
    IEnumerable<ISendable> Process(IPublicMessageReceived publicMessageReceived);
    IEnumerable<ISendable> Process(IMuteReceived muteReceived);
    IEnumerable<ISendable> Process(IUnMuteBanReceived unMuteBanReceived);
    IEnumerable<ISendable> Process(ISubonlyReceived subonlyReceived);
    IEnumerable<ISendable> Process(IBanReceived banReceived);
    IEnumerable<ISendable> Process(IBroadcastReceived broadcastReceived);
  }
}
