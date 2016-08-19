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
        new PublicMessageReceived("!long", true),
        new PublicMessageReceived("hi"),
        new PublicMessageReceived("banplox"),
        new PublicMessageReceived("!time"),
        new PublicMessageReceived("!sing", true),
      };
      var sender = new ConsoleSender();
      var receiver = new SampleReceiver(received, sender);

      var banScanner = new ScanForBans();
      var commandScanner = new ScanForCommands();
      var modCommandScanner = new ScanForModCommands();
      var messageProcessor = new MessageProcessor(banScanner, commandScanner, modCommandScanner);

      var receivedProcessor = new ReceivedProcessor(messageProcessor);
      receiver.Run(receivedProcessor);

      Console.ReadLine();
    }
  }
}
