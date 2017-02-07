using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  [TestClass]
  public class StateIntegerRepositoryTests : BaseRepositoryTests {

    [TestMethod]
    public void ReadWriteLatestStreamOnTime() {
      var testWrite = DateTime.Now;

      using (var context = new BotDbContext()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        stateIntegerRepository.LatestStreamOnTime = testWrite;
        context.SaveChanges();
      }

      DateTime testRead;
      using (var context = new BotDbContext()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        testRead = stateIntegerRepository.LatestStreamOnTime = testWrite;
      }

      Assert.AreEqual(testWrite, testRead);
    }

    [TestMethod]
    public void ReadWriteLatestStreamOffTime() {
      var testWrite = DateTime.Now;

      using (var context = new BotDbContext()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        stateIntegerRepository.LatestStreamOffTime = testWrite;
        context.SaveChanges();
      }

      DateTime testRead;
      using (var context = new BotDbContext()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        testRead = stateIntegerRepository.LatestStreamOffTime = testWrite;
      }

      Assert.AreEqual(testWrite, testRead);
    }

    [TestMethod]
    public void ReadWriteDeathCount() {
      var testWrite = new Random().Next();

      using (var context = new BotDbContext()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        stateIntegerRepository.DeathCount = testWrite;
        context.SaveChanges();
      }

      long testRead;
      using (var context = new BotDbContext()) {
        var stateIntegerRepository = new StateIntegerRepository(context.StateIntegers);
        testRead = stateIntegerRepository.DeathCount = testWrite;
      }

      Assert.AreEqual(testWrite, testRead);
    }

  }
}
