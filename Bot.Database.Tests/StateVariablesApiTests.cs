using System;
using Bot.Pipeline;
using Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {

  [TestClass]
  public class StateVariablesApiTests {

    [TestMethod]
    public void GetAndSetOnTime() {
      // Arrange
      var consoleLogger = new ConsoleLogger();
      var factory = new ApiFactory(consoleLogger);
      var api = factory.GetStateIntegerApi;
      var expected = DateTime.UtcNow;

      // Act
      api.LatestStreamOnTime = expected;
      var actual = api.LatestStreamOnTime;

      // Assert
      Assert.AreEqual(expected.ToUnixTime(), actual.ToUnixTime()); // Need Unix time to round; 

    }
  }
}
