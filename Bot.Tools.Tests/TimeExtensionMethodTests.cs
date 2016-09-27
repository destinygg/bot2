using System;
using Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Tools.Tests {

  [TestClass]
  public class TimeExtensionMethodTests {

    [TestMethod]
    public void ToUnixTime_year2k() {
      // Arrange
      var year2K = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      var expected = 946684800L;

      // Act
      var actual = year2K.ToUnixTime();

      // Assert
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void FromUnixTime_year2k() {
      // Arrange
      var year2K = 946684800L;
      var expected = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

      // Act
      var actual = year2K.FromUnixTime();

      // Assert
      Assert.AreEqual(expected, actual);
    }

  }
}
