using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {

  public class ScanForModCommands : IScanForModCommands {
    public IEnumerable<ISendable> Scan(IMessageReceived message, IEnumerable<IPublicMessageReceived> context) {
      context = context.ToList();
      var outbox = new List<ISendable>();
      if (!message.Sender.IsMod) return outbox;
      if (message.StartsWith("!sing"))
        outbox.Add(new PublicMessage("/me sings a song"));
      if (message.StartsWith("!long")) {
        Console.WriteLine("long begin" + context.Count(x => x.Text != ""));
        for (var i = 0; i < 1000000000; i++) {
          var temp = i;
        }
        Console.WriteLine("1");
        for (var i = 0; i < 1000000000; i++) {
          var temp = i;
        }
        Console.WriteLine("2");
        for (var i = 0; i < 1000000000; i++) {
          var temp = i;
        }
        Console.WriteLine("3");
        for (var i = 0; i < 1000000000; i++) {
          var temp = i;
        }
        Console.WriteLine("long over" + context.Count(x => x.Text != ""));
      }
      return outbox;
    }
  }
}
