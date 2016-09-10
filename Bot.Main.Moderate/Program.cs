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

      var banScanner = new ScanForBans();
      var commandScanner = new ScanForCommands();
      var modCommandScanner = new ScanForModCommands();
      var messageProcessor = new ContextualizedProcessor(banScanner, commandScanner, modCommandScanner);

      var receivedProcessor = new ReceivedProcessor(messageProcessor);
      receiver.Run(receivedProcessor);

      var contextualizedProducer = new ContextualizedProducer(receiver.Produce);
      var contextualizedBlock = contextualizedProducer.Produce;

      var sendableProducer = new SendableProducer(contextualizedBlock, messageProcessor);
      var sendableBlock = sendableProducer.Produce;

      var sender = new ConsoleSender(sendableBlock);
      sender.Run();

      Console.ReadLine();
    }
  }
}
