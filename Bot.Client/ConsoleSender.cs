using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Client.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class ConsoleSender : ISender {
    public void Send(IPublicMessage publicMessage) {
      Console.WriteLine($"Sending a public message: {publicMessage.Text}");
    }

    public void Send(IMute mute) {
      Console.WriteLine($"Sending a mute targeting {mute.Target} for {mute.Duration}");
    }

    public void Send(IUnMuteBan unMuteBan) {
      Console.WriteLine($"Sending an unmuteban targeting {unMuteBan.Target}");
    }

    public void Send(ISubonly subonly) {
      Console.WriteLine($"Sending a subonly that is {subonly.IsEnabled}");
    }

    public void Send(IBan ban) {
      Console.WriteLine($"Sending a ban targeting {ban.Target} for {ban.Duration}; perm is {ban.IsPermanent}");
    }

    public void Send(IBroadcast broadcast) {
      Console.WriteLine($"Sending a broadcast saying: {broadcast.Text}");
    }
  }
}
