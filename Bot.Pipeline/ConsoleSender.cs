using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class ConsoleSender : ISender {

    //Console.WriteLine($"Sending a mute targeting {mute.Target} for {mute.Duration}");

    //Console.WriteLine($"Sending a pardon targeting {pardon.Target}");

    //Console.WriteLine($"Sending a subonly that is {subonly.IsEnabled}");

    //Console.WriteLine($"Sending a ban targeting {ban.Target} for {ban.Duration}; perm is {ban.IsPermanent}");

    //Console.WriteLine($"Sending a broadcast saying: {broadcast.Text}");

    public void Send(ISendableProducer sendableProducer) {
      var actionBlock = new ActionBlock<IReadOnlyList<ISendable>>(r => _send(r));
      sendableProducer.SendableBlock.LinkTo(actionBlock);
    }

    private void _send(IEnumerable<ISendable> sendables) {
      foreach (var sendable in sendables) {
        Console.WriteLine(sendable);
      }
    }

  }
}
