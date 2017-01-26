using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Api.Tests {
  [TestClass]
  public class StateIntegerApiTests {


    [TestInitialize]
    public void Initialize() {
      var manager = new DatabaseManager();
      manager.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup() {
      var manager = new DatabaseManager();
      manager.EnsureDeleted();
    }

    [TestMethod]
    public void ReadWriteLatestStreamOnTime() {
      var testWrite = DateTime.Now;

      using (var context = new BotDbContext()) {
        var stateIntegerApi = new StateIntegerApi(context.StateIntegers);
        stateIntegerApi.LatestStreamOnTime = testWrite;
        context.SaveChanges();
      }

      DateTime testRead;
      using (var context = new BotDbContext()) {
        var stateIntegerApi = new StateIntegerApi(context.StateIntegers);
        testRead = stateIntegerApi.LatestStreamOnTime = testWrite;
      }

      Assert.AreEqual(testWrite, testRead);
    }

    [TestMethod]
    public void ReadWriteLatestStreamOffTime() {
      var testWrite = DateTime.Now;

      using (var context = new BotDbContext()) {
        var stateIntegerApi = new StateIntegerApi(context.StateIntegers);
        stateIntegerApi.LatestStreamOffTime = testWrite;
        context.SaveChanges();
      }

      DateTime testRead;
      using (var context = new BotDbContext()) {
        var stateIntegerApi = new StateIntegerApi(context.StateIntegers);
        testRead = stateIntegerApi.LatestStreamOffTime = testWrite;
      }

      Assert.AreEqual(testWrite, testRead);
    }

    [TestMethod]
    public void ReadWriteDeathCount() {
      var testWrite = new Random().Next();

      using (var context = new BotDbContext()) {
        var stateIntegerApi = new StateIntegerApi(context.StateIntegers);
        stateIntegerApi.DeathCount = testWrite;
        context.SaveChanges();
      }

      long testRead;
      using (var context = new BotDbContext()) {
        var stateIntegerApi = new StateIntegerApi(context.StateIntegers);
        testRead = stateIntegerApi.DeathCount = testWrite;
      }

      Assert.AreEqual(testWrite, testRead);
    }

  }
}
