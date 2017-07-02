using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class TestableLoggerTests {

    private const string Debug = "Debug";
    private const string Information = "Information";
    private const string Warning = "Warning";
    private const string Error = "Error";
    private const string Fatal = "Fatal";

    private static TestableLogger _createFullyPopulatedTestableLogger() {
      var testableLogger = new TestableLogger();
      testableLogger.LogDebug(Debug);
      testableLogger.LogInformation(Information);
      testableLogger.LogWarning(Warning);
      testableLogger.LogError(Error);
      testableLogger.LogFatal(Fatal);
      return testableLogger;
    }

    [TestMethod]
    public void TestableLogger_DebugOutbox_ReturnsDebugAndLower() {
      var testableLogger = _createFullyPopulatedTestableLogger();

      var debugOutbox = testableLogger.DebugOutbox;

      Assert.IsTrue(debugOutbox.SequenceEqual(new List<string> { Debug, Information, Warning, Error, Fatal }));
    }

    [TestMethod]
    public void TestableLogger_InformationOutbox_ReturnsInformationAndLower() {
      var testableLogger = _createFullyPopulatedTestableLogger();

      var debugOutbox = testableLogger.InformationOutbox;

      Assert.IsTrue(debugOutbox.SequenceEqual(new List<string> { Information, Warning, Error, Fatal }));
    }

    [TestMethod]
    public void TestableLogger_WarningOutbox_ReturnsWarningAndLower() {
      var testableLogger = _createFullyPopulatedTestableLogger();

      var debugOutbox = testableLogger.WarningOutbox;

      Assert.IsTrue(debugOutbox.SequenceEqual(new List<string> { Warning, Error, Fatal }));
    }

    [TestMethod]
    public void TestableLogger_ErrorOutbox_ReturnsErrorAndLower() {
      var testableLogger = _createFullyPopulatedTestableLogger();

      var debugOutbox = testableLogger.ErrorOutbox;

      Assert.IsTrue(debugOutbox.SequenceEqual(new List<string> { Error, Fatal }));
    }

    [TestMethod]
    public void TestableLogger_FatalOutbox_ReturnsFatalAndLower() {
      var testableLogger = _createFullyPopulatedTestableLogger();

      var debugOutbox = testableLogger.FatalOutbox;

      Assert.IsTrue(debugOutbox.SequenceEqual(new List<string> { Fatal }));
    }

  }
}
