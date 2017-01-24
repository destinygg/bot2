using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Api.Tests {
  [TestClass]
  public class StateIntegerApiTests {
    [TestMethod]
    public void InitializeDb() {
      new InitializeDb();
    }

    [TestMethod]
    public void LatestLiveTimeUpdate() {
      var s = new StateIntegerApi();
      s.LatestLiveTime = DateTime.UtcNow;
    }

    [TestMethod]
    public void LatestLiveTimeRead() {
      var s = new StateIntegerApi();
      var time = s.LatestLiveTime;
    }
  }
}
