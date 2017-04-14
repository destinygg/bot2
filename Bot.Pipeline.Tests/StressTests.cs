using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class StressTests {
    [TestMethod]
    public void TwentyFiveCivilianMessages_WithNuke_ProcessesInUnder20ms() {
      var outputStopwatch = new Stopwatch();
      var outputMilliseconds = new List<long>();
      var outputStrings = new List<string>();
      var sender = new TestableSender(s => {
        outputMilliseconds.Add(outputStopwatch.ElapsedMilliseconds);
        outputStrings.Add(s);
      });
      var containerManager = new TestContainerManager(container => {
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, container);
        container.RegisterConditional(typeof(ICommandHandler<IEnumerable<ISendable<ITransmittable>>>), timeServiceRegistration, _ => true);
      }).InitializeAndIsolateRepository();
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipeline = containerManager.GetInstance<IPipeline>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!nuke burrito"),
        factory.PublicReceivedMessage("User01","burrito"),
        factory.PublicReceivedMessage("User02","burrito"),
        factory.PublicReceivedMessage("User03","burrito"),
        factory.PublicReceivedMessage("User04","burrito"),
        factory.PublicReceivedMessage("User05","burrito"),
        factory.PublicReceivedMessage("User06","burrito"),
        factory.PublicReceivedMessage("User07","burrito"),
        factory.PublicReceivedMessage("User08","burrito"),
        factory.PublicReceivedMessage("User09","burrito"),
        factory.PublicReceivedMessage("User10","burrito"),
        factory.PublicReceivedMessage("User11","burrito"),
        factory.PublicReceivedMessage("User12","burrito"),
        factory.PublicReceivedMessage("User13","burrito"),
        factory.PublicReceivedMessage("User14","burrito"),
        factory.PublicReceivedMessage("User15","burrito"),
        factory.PublicReceivedMessage("User16","burrito"),
        factory.PublicReceivedMessage("User17","burrito"),
        factory.PublicReceivedMessage("User18","burrito"),
        factory.PublicReceivedMessage("User19","burrito"),
        factory.PublicReceivedMessage("User20","burrito"),
        factory.PublicReceivedMessage("User21","burrito"),
        factory.PublicReceivedMessage("User22","burrito"),
        factory.PublicReceivedMessage("User23","burrito"),
        factory.PublicReceivedMessage("User24","burrito"),
        factory.PublicReceivedMessage("User25","burrito"),
      };

      var enqueueStopwatch = new Stopwatch();
      enqueueStopwatch.Start();
      outputStopwatch.Start();
      data.ForEach(x => {
        pipeline.Enqueue(x);
      });
      enqueueStopwatch.Stop();

      Task.Delay(10000).Wait();
      Console.WriteLine($"Enqueue Total: {enqueueStopwatch.ElapsedMilliseconds}");
      Console.WriteLine($"Output Range : {outputMilliseconds.Min()} - {outputMilliseconds.Max()}");
      Console.WriteLine($"Output Range : {outputStrings.Min()} - {outputStrings.Max()}");
      Assert.IsTrue(outputMilliseconds.Max() - outputMilliseconds.Min() < 20);
      Assert.IsTrue(enqueueStopwatch.ElapsedMilliseconds < 20);
      Assert.IsTrue(outputStrings.Count > 15);
    }

  }
}
