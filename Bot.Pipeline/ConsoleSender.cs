using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class ConsoleSender : ISender {
    private readonly ISourceBlock<IReadOnlyList<ISendable>> _sendableBlock;

    public ConsoleSender(ISourceBlock<IReadOnlyList<ISendable>> sendableBlock) {
      _sendableBlock = sendableBlock;
    }

    //Console.WriteLine($"Sending a mute targeting {mute.Target} for {mute.Duration}");

    //Console.WriteLine($"Sending an unmuteban targeting {unMuteBan.Target}");

    //Console.WriteLine($"Sending a subonly that is {subonly.IsEnabled}");

    //Console.WriteLine($"Sending a ban targeting {ban.Target} for {ban.Duration}; perm is {ban.IsPermanent}");

    //Console.WriteLine($"Sending a broadcast saying: {broadcast.Text}");

    private void Send(IReadOnlyList<ISendable> sendables) {
      foreach (var sendable in sendables) {
        Console.WriteLine(sendable.ConsolePrint);
      }
    }

    public void Run() {
      var transform = new ActionBlock<IReadOnlyList<ISendable>>(r => Send(r), new ExecutionDataflowBlockOptions {
        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
      });
      _sendableBlock.LinkTo(transform);
    }
  }
}
