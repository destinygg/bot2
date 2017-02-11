using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Diagnostics;

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

    [TestMethod]
    public void Container_Always_ContainsNoDiagnosticWarnings() {
      // Arrange
      var containerManager = new ContainerManager();
      var container = (Container) _GetInstanceField(typeof(ContainerManager), containerManager, "_container");

      // Act
      container.Verify(VerificationOption.VerifyOnly);

      // Assert
      var diagnosticResults = Analyzer.Analyze(container);
      Assert.IsFalse(diagnosticResults.Any(), Environment.NewLine +
        string.Join(Environment.NewLine, diagnosticResults.Select(result => result.Description)));
    }

    private object _GetInstanceField(Type type, object instance, string fieldName) {
      const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
      var field = type.GetField(fieldName, bindFlags);
      return field.GetValue(instance);
    }

  }
}
