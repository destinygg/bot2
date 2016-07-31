using System.Collections.Generic;
using Bot.Client.Contracts;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class SampleClient {
    private readonly IReceiver _receiver;

    public SampleClient(IReceiver receiver) {
      _receiver = receiver;
    }

    public void Run(IReceivedProcessor receivedProcessor) {
      _receiver.Run(receivedProcessor);
    }
  }
}
