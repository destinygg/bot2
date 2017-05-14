using System;
using System.Linq;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var firstArg = args.FirstOrDefault();

      if (string.IsNullOrWhiteSpace(firstArg)) {
        Console.WriteLine("Select a client:");
        Console.WriteLine("");
        Console.WriteLine("s   = sample client");
        firstArg = Console.ReadLine();
      }

      IExecutable executable;

      switch (firstArg) {
        case "s":
          executable = new SampleData();
          break;
        default:
          throw new Exception("Invalid input");
      }

      executable.Execute();

      Console.ReadLine();
    }
  }
}
