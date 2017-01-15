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
      var t = now.IsBeforeAndWithin(now, TimeSpan.FromSeconds(1));

      // Assert
      Assert.IsTrue(t);
    }

    [TestMethod]
    public void IsBeforeAndWithin_After() {
      // Arrange
      var now = DateTime.UtcNow;
      var after = now + TimeSpan.FromSeconds(1);

      // Act
      var f = after.IsBeforeAndWithin(now, TimeSpan.FromSeconds(2));

      // Assert
      Assert.IsFalse(f);
    }

    [TestMethod]
    public void IsBeforeAndWithin_Before_True() {
      // Arrange
      var now = DateTime.UtcNow;
      var before = now - TimeSpan.FromSeconds(1);

      // Act
      var t = before.IsBeforeAndWithin(now, TimeSpan.FromSeconds(2));

      // Assert
      Assert.IsTrue(t);
    }

    [TestMethod]
    public void IsBeforeAndWithin_Before_False() {
      // Arrange
      var now = DateTime.UtcNow;
      var before = now - TimeSpan.FromSeconds(3);

      // Act
      var f = before.IsBeforeAndWithin(now, TimeSpan.FromSeconds(2));

      // Assert
      Assert.IsFalse(f);
    }

  }
}
