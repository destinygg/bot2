using System;
using System.Diagnostics;
using System.Linq;
using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  [Ignore]
  public class StressTests {

    private const int Loops = 100;

    private void _printLoopTime(Action<IQueryCommandService<IUnitOfWork>> action) {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var unit = container.GetInstance<IQueryCommandService<IUnitOfWork>>();

      var stopwatch = new Stopwatch();
      stopwatch.Start();
      foreach (var i in Enumerable.Range(1, Loops)) {
        action(unit);
      }
      stopwatch.Stop();
      Console.WriteLine(stopwatch.ElapsedMilliseconds + "total");
      Console.WriteLine(stopwatch.ElapsedMilliseconds / Loops + " each");
    }

    [TestMethod]
    public void _100_Autopunishment_Reads() {
      _printLoopTime(uom => uom.Query(u => u.AutoPunishments.GetAllWithUser));
    }

    [TestMethod]
    public void _100_Autopunishment_Writes() {
      _printLoopTime(
        uom => uom.Command(u => u.AutoPunishments.Add(
          new Models.AutoPunishment(new AutoPunishmentEntity {
            Term = TestHelper.RandomString(),
            Duration = (long) TimeSpan.FromMinutes(10).TotalSeconds,
            Type = AutoPunishmentType.BannedRegex
          }))));
    }

    [TestMethod]
    public void _100_StateInteger_Reads() {
      _printLoopTime(uom => uom.Query(u => u.StateIntegers.LatestStreamOffTime));
    }

    [TestMethod]
    public void _100_StateInteger_Writes() {
      _printLoopTime(uom => uom.Command(u => u.StateIntegers.LatestStreamOffTime = DateTime.Now));
    }

  }
}
