using System.Net;
using Bot.Logic.Interfaces;
using Bot.Main.Moderate;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class DownloaderMapperTests {

    [TestMethod]
    public void DownloadMapper_OverRustleLogsExistingUser_DoesNotCrash_DoNotRunContinuously() {
      var testContainerManager = new TestContainerManager();
      var downloadMapper = testContainerManager.Container.GetInstance<IDownloadMapper>();

      var logs = downloadMapper.OverRustleLogs("woopboop");

      Assert.IsNotNull(logs);
    }

    [TestMethod]
    public void DownloadMapper_OverRustleLogsNonexistantUser_404s_DoNotRunContinuously() {
      var testContainerManager = new TestContainerManager();
      var downloadFactory = testContainerManager.Container.GetInstance<IDownloadMapper>();

      var exception = TestHelper.AssertCatch<WebException>(() => downloadFactory.OverRustleLogs(TestHelper.RandomString()));

      Assert.AreEqual(((HttpWebResponse) exception.Response).StatusCode, HttpStatusCode.NotFound);
    }

  }
}
