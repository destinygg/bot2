using System.Net;
using Bot.Main.Moderate;
using Bot.Pipeline.Tests;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Tools.Tests {
  [TestClass]
  public class DownloadFactoryTests {
    [TestMethod]
    public void DownloadFactory_Never_Logs() {
      var testableLogger = new TestableLogger();
      var testContainerManager = new TestContainerManager(c => {
        var loggerRegistration = Lifestyle.Singleton.CreateRegistration(() => testableLogger, c);
        c.RegisterConditional(typeof(ILogger), loggerRegistration, pc => !pc.Handled);
      });
      var downloadFactory = testContainerManager.Container.GetInstance<IFactory<string, string, string>>();

      TestHelper.AssertCatch<WebException>(() => downloadFactory.Create("https://httpbin.org/404", ""));

      Assert.AreEqual(0, testableLogger.Outbox.Count);
    }
  }
}
