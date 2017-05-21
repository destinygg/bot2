using System;
using System.Linq;
using log4net.Core;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var inputExecutable = args.FirstOrDefault();
      var inputLevel = args.Skip(1).FirstOrDefault();

      var executable = ConfigureExecutable(inputExecutable);
      var level = ConfigureLogger(inputLevel);
      var logger = new SetupLog4Net();
      logger.Setup(level);
      executable.Execute();

      Console.ReadLine();
    }

    private static IExecutable ConfigureExecutable(string inputClient) {
      if (string.IsNullOrWhiteSpace(inputClient)) {
        Console.WriteLine("Select an executable:");
        Console.WriteLine("dl  = destiny.gg - listening ");
        Console.WriteLine("dlt = destiny.gg - listening and twitter");
        Console.WriteLine("ds  = destiny.gg - sending ");
        Console.WriteLine("dst = destiny.gg - sending and twitter");
        Console.WriteLine("tl  = twitch     - listening ");
        Console.WriteLine("tlt = twitch     - listening and twitter");
        Console.WriteLine("ts  = twitch     - sending ");
        Console.WriteLine("tst = twitch     - sending and twitter");
        Console.WriteLine("s   = client with sample data");
        inputClient = Console.ReadLine();
      }

      IExecutable executable;
      switch (inputClient) {
        case "dl":
          executable = new DestinyGgExecutable(false, false, true);
          break;
        case "dlt":
          executable = new DestinyGgExecutable(false, true, true);
          break;
        case "ds":
          executable = new DestinyGgExecutable(true, false, true);
          break;
        case "dst":
          executable = new DestinyGgExecutable(true, true, true);
          break;
        case "tl":
          executable = new DestinyGgExecutable(false, false, false);
          break;
        case "tlt":
          executable = new DestinyGgExecutable(false, true, false);
          break;
        case "ts":
          executable = new DestinyGgExecutable(true, false, false);
          break;
        case "tst":
          executable = new DestinyGgExecutable(true, true, false);
          break;
        case "s":
          executable = new SampleDataExecutable();
          break;
        default:
          throw new Exception("Invalid input");
      }
      return executable;
    }

    private static Level ConfigureLogger(string inputLevel) {
      if (string.IsNullOrWhiteSpace(inputLevel)) {
        Console.WriteLine("Select a logging level:");
        Console.WriteLine("o = off");
        Console.WriteLine("f = fatal");
        Console.WriteLine("e = error");
        Console.WriteLine("w = warn");
        Console.WriteLine("i = info");
        Console.WriteLine("d = debug");
        Console.WriteLine("a = all");
        inputLevel = Console.ReadLine();
      }

      Level level;
      switch (inputLevel) {
        case "o":
          level = Level.Off;
          break;
        case "f":
          level = Level.Fatal;
          break;
        case "e":
          level = Level.Error;
          break;
        case "w":
          level = Level.Warn;
          break;
        case "i":
          level = Level.Info;
          break;
        case "d":
          level = Level.Debug;
          break;
        case "a":
          level = Level.All;
          break;
        default:
          throw new Exception("Invalid input");
      }
      return level;
    }

  }
}
