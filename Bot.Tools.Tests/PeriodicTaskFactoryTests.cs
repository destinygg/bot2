using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Tools.Tests {
  [TestClass]
  public class PeriodicTaskFactoryTests {

    [TestMethod]
    public void PeriodicTaskFactory_TwoCreates_RunInParallel() {
      var factory = new PeriodicTaskFactory();
      var listA = new List<long>();
      var listB = new List<long>();
      var stopwatchA = new Stopwatch();
      var stopwatchB = new Stopwatch();
      stopwatchA.Start();
      stopwatchB.Start();

      factory.Create(TimeSpan.FromMilliseconds(100), () => listA.Add(stopwatchA.ElapsedMilliseconds));
      factory.Create(TimeSpan.FromMilliseconds(200), () => listB.Add(stopwatchA.ElapsedMilliseconds));

      Task.Delay(1000).Wait();
      foreach (var i in listA) {
        Console.WriteLine("A:" + i);
      }
      foreach (var i in listB) {
        Console.WriteLine("B:" + i);
      }
      Console.WriteLine(listA.Count);
      Console.WriteLine(listB.Count);
      Assert.AreEqual(10, listA.Count);
      Assert.AreEqual(5, listB.Count);
    }

  }
}
