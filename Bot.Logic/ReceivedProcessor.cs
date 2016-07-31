using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ReceivedProcessor : IReceivedProcessor {

    public void Process(IPrivateMessageReceived privateMessageReceived) {
      
    }

    public void Process(IPublicMessageReceived publicMessageReceived) {
      
    }

    public void Process(IMuteReceived muteReceived) {
      
    }

    public void Process(IUnMuteBanReceived unMuteBanReceived) {
      
    }

    public void Process(ISubonlyReceived subonlyReceived) {
      
    }

    public void Process(IBanReceived banReceived) {
      
    }

    public void Process(IBroadcastReceived broadcastReceived) {
      
    }
  }
}
