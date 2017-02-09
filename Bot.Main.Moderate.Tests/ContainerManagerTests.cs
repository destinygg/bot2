using System.Reflection;
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

    [TestMethod]
    public void InvokeAllPublicProperties() {
      // Arrange
      var containerManager = new ContainerManager();

      // Act
      var propertyInfos = typeof(ContainerManager).GetProperties(BindingFlags.Public | BindingFlags.Instance);
      foreach (var propertyInfo in propertyInfos) {
        var value = propertyInfo.GetValue(containerManager);

        // Assert
        Assert.IsNotNull(value);
      }
    }

  }
}
