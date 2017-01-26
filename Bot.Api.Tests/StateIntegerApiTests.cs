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

  }
}
