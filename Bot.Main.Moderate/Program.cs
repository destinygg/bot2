using System;
using System.Collections.Generic;
using Bot.Logic;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline;

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var received = new List<IReceived>() {
        new PublicMessageReceived("!long", true),
        new PublicMessageReceived("hi"),
        new PublicMessageReceived("banplox"),
        new PublicMessageReceived("!time"),
        new PublicMessageReceived("!sing", true),
        new PublicMessageReceived("!long", true),
      };

      var receiver = new SampleReceivedProducer(received);
      var contextualizedProducer = new ContextualizedProducer(receiver.Produce);
      var contextualizedProcessor = new ContextualizedProcessor(new ScanForBans(), new ScanForCommands(), new ScanForModCommands());
      var sendableProducer = new SendableProducer(contextualizedProducer.Produce, contextualizedProcessor);
      var sender = new ConsoleSender(sendableProducer.Produce);

      receiver.Run();
      sender.Run();

      Console.ReadLine();
    }
  }
}
