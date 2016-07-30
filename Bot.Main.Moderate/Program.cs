using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Client;
using Bot.Logic;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var received = new List<IReceived>() {
        new PublicMessageReceived { Text = "hi" },
      };
      var messageProcessor = new MessageProcessor();
      var client = new SampleClient(received, messageProcessor);
      client.Run();
      Console.WriteLine("borkbork");
      Console.ReadLine();
    }
  }
}
