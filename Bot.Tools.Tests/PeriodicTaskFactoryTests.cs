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

      factory.Create(TimeSpan.FromMilliseconds(50), () => listA.Add(stopwatchA.ElapsedMilliseconds));
      factory.Create(TimeSpan.FromMilliseconds(100), () => listB.Add(stopwatchA.ElapsedMilliseconds));

      Task.Delay(140).Wait();

      Assert.AreEqual(3, listA.Count); // 0ms, ~50+ ms, ~100+ ms
      Assert.AreEqual(2, listB.Count); // 0ms,          ~100+ ms
    }

  }
}
