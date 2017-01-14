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
    public void OrderInvariant() {
      // Arrange
      var stringA = "The quick brown fox jumped";
      var stringB = "The quick brown fox jumped over the lazy dog.";

      // Act
      var semi1 = stringA.SimilarTo(stringB);
      var semi2 = stringB.SimilarTo(stringA);

      // Assert
      Assert.AreEqual(semi1, semi2);
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

    [TestMethod]
    public void CaseInsensitive() {
      // Arrange
      var stringA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      var stringB = "abcdefghijklmnopqrstuvwxyz";

      // Act
      var one = stringA.SimilarTo(stringB);

      // Assert
      Assert.AreEqual(1, one);
    }

  }
}
