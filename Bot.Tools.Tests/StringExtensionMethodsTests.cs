using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Tools.Tests {

  [TestClass]
  public class StringExtensionMethodsTests {

    [TestMethod]
    public void PefectMatch() {
      // Arrange
      var stringA = "The quick brown fox jumped over the lazy dog.";
      var stringB = "The quick brown fox jumped over the lazy dog.";

      // Act
      var one = stringA.SimilarTo(stringB);

      // Assert
      Assert.AreEqual(1, one);
    }

    [TestMethod]
    public void PartialMatch() {
      // Arrange
      var stringA = "The quick brown fox jumped";
      var stringB = "The quick brown fox jumped over the lazy dog.";

      // Act
      var semi = stringA.SimilarTo(stringB);

      // Assert
      Assert.IsTrue(0 < semi && semi < 1);
    }

    [TestMethod]
    public void ZeroMatch() {
      // Arrange
      var stringA = "";
      var stringB = "The quick brown fox jumped over the lazy dog.";

      // Act
      var zero = stringA.SimilarTo(stringB);

      // Assert
      Assert.AreEqual(0, zero);
    }

  }
}
