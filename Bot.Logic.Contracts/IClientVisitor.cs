using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IClientVisitor {
    void Visit(IPrivateMessageReceived privateMessageReceived);
    void Visit(IPublicMessageReceived publicMessageReceived);
    void Visit(IMuteReceived muteReceived);
    void Visit(IUnMuteBanReceived unMuteBanReceived);
    void Visit(ISubonlyReceived subonlyReceived);
    void Visit(IBanReceived banReceived);
    void Visit(IBroadcastReceived broadcastReceived);
  }
}
