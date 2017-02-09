using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Main.Moderate.Tests {
  [TestClass]
  public class ContainerManagerTests {

    [TestMethod]
    public void VerifyContainer() {
      // Arrange
      // Act
      var containerManager = new ContainerManager();

      // Assert
      Assert.IsNotNull(containerManager);
    }

  }
}
