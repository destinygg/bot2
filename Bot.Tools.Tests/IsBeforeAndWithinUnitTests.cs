using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Tools.Tests {
  [TestClass]
  public class IsBeforeAndWithinUnitTests {
    [TestMethod]
    public void IsBeforeAndWithin() {
      // Arrange
      var now = DateTime.UtcNow;

      // Act
      var expected = now.IsBeforeAndWithin(TimeSpan.FromSeconds(1));

      // Assert
      Assert.IsTrue(expected);
    }

    [TestMethod]
    public void IsBeforeAndWithin_After() {
      // Arrange
      var after = DateTime.UtcNow + TimeSpan.FromSeconds(1);

      // Act
      var expected = after.IsBeforeAndWithin(TimeSpan.FromSeconds(2));

      // Assert
      Assert.IsFalse(expected);
    }

    [TestMethod]
    public void IsBeforeAndWithin_Before_True() {
      // Arrange
      var before = DateTime.UtcNow - TimeSpan.FromSeconds(1);

      // Act
      var expected = before.IsBeforeAndWithin(TimeSpan.FromSeconds(2));

      // Assert
      Assert.IsTrue(expected);
    }

    [TestMethod]
    public void IsBeforeAndWithin_Before_False() {
      // Arrange
      var before = DateTime.UtcNow - TimeSpan.FromSeconds(3);

      // Act
      var expected = before.IsBeforeAndWithin(TimeSpan.FromSeconds(2));

      // Assert
      Assert.IsFalse(expected);
    }

  }
}
