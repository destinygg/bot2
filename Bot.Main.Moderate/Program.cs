using System;
using System.Collections.Generic;
using Bot.Client;
using Bot.Logic;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var received = new List<IReceived>() {
        new PublicMessageReceived("hi"),
      };
      var sender = new ConsoleSender();
      var receiver = new SampleReceiver(received);
      var receivedProcessor = new ReceivedProcessor(sender);
      receiver.Run(receivedProcessor);

      Console.ReadLine();
    }
  }
}
