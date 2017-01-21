using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class SampleReceivedProducer : IReceivedProducer {
    private readonly IEnumerable<IReceived> _received;

    public SampleReceivedProducer(IReceivedFactory factory) {
      _received = new List<IReceived> {
        factory.ModPublicReceivedMessage("!long"),
        factory.PublicReceivedMessage("hi"),
        factory.PublicReceivedMessage("banplox"),
        factory.PublicReceivedMessage("!time"),
        factory.ModPublicReceivedMessage("!sing"),
        factory.ModPublicReceivedMessage("!long"),
      };
      ReceivedBlock = new BufferBlock<IReceived>();
      Task.Factory.StartNew(Run);
    }

    private async void Run() {
      var k = 1;
      while (true) {
        await Task.Delay(1000);
        ReceivedBlock.Post(new PublicReceivedMessage("Post: hi", DateTime.Now));
        await ReceivedBlock.SendAsync(new PublicReceivedMessage("SendAsync: hi", DateTime.Now));
        Console.WriteLine("Send: {0}", k);
        k++;
      }
    }

    public BufferBlock<IReceived> ReceivedBlock { get; }

  }
}
