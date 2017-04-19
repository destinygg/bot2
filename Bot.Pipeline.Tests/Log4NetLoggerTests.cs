using System;
using System.IO;
using Bot.Tests;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class Log4NetLoggerTests {

    [TestMethod]
    public void Log4NetLogger_Displays_Exceptions() {
      XmlConfigurator.Configure(new FileInfo(@"log4net.config"));
      var containerManager = new TestContainerManager();
      var log4NetLogger = containerManager.Container.GetInstance<Log4NetLogger<Log4NetLoggerTests>>();
      try {
        var zero = 0;
        var undefined = 1 / zero;
      } catch (Exception e) {
        log4NetLogger.LogError("An error message", e);
      }
    }

  }
}
