using Bot.Logic.Interfaces;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class DownloaderTests {

    [TestMethod]
    public void Downloader_OverRustleLogs_DoesNotCrash() {
      var testContainerManager = new TestContainerManager();
      var downloadFactory = testContainerManager.Container.GetInstance<IDownloader>();

      var html = downloadFactory.OverRustleLogs("woopboop");

      Assert.IsNotNull(html);
    }

  }
}
