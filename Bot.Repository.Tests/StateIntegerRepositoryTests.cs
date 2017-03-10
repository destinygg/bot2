using System;
using Bot.Database;
using Bot.Tests;
using Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class StateIntegerRepositoryTests {

    [TestMethod]
    public void ReadWriteLatestStreamOnTime() {
      var container = RepositoryHelper.GetContainer(nameof(ReadWriteLatestStreamOnTime));

      // todo fix
      var testWrite = ((long) TestHelper.RandomInt()).FromUnixTime();
      using (var context = container.GetInstance<BotDbContext>()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        stateIntegerRepository.LatestStreamOnTime = testWrite;
        context.SaveChanges();
      }

      DateTime testRead;
      using (var context = container.GetInstance<BotDbContext>()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        testRead = stateIntegerRepository.LatestStreamOnTime;
      }

      Assert.AreEqual(testWrite.Ticks, testRead.Ticks);
    }

    [TestMethod]
    public void ReadWriteLatestStreamOffTime() {
      var container = RepositoryHelper.GetContainer(nameof(ReadWriteLatestStreamOffTime));

      var testWrite = DateTime.FromBinary(TestHelper.RandomInt());
      using (var context = container.GetInstance<BotDbContext>()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        stateIntegerRepository.LatestStreamOffTime = testWrite;
        context.SaveChanges();
      }

      DateTime testRead;
      using (var context = container.GetInstance<BotDbContext>()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        testRead = stateIntegerRepository.LatestStreamOffTime;
      }

      Assert.AreEqual(testWrite.Ticks, testRead.Ticks);
    }

    [TestMethod]
    public void ReadWriteDeathCount() {
      var container = RepositoryHelper.GetContainer(nameof(ReadWriteDeathCount));

      var testWrite = TestHelper.RandomInt();
      using (var context = container.GetInstance<BotDbContext>()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        stateIntegerRepository.DeathCount = testWrite;
        context.SaveChanges();
      }

      long testRead;
      using (var context = container.GetInstance<BotDbContext>()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        testRead = stateIntegerRepository.DeathCount;
      }

      Assert.AreEqual(testWrite, testRead);
    }

  }
}
