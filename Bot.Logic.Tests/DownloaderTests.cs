using Bot.Logic.Interfaces;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class DownloaderTests {

    [TestMethod]
    public void Downloader_OverRustleLogs_DoesNotCrash() {
      var testContainerManager = new TestContainerManager();
      var downloader = testContainerManager.Container.GetInstance<IDownloader>();

      var html = downloader.OverRustleLogs("woopboop");

      Assert.IsNotNull(html);
    }

  }
}
