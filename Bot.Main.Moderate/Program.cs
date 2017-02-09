using System;

namespace Bot.Main.Moderate {
  class Program {
    static void Main(string[] args) {
      var containerManager = new ContainerManager();
      var data = containerManager.SampleReceived;
      var pipeline = containerManager.Pipeline;

      pipeline.Run(data);

      Console.ReadLine();
    }
  }
}
