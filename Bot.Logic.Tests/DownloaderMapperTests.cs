using Bot.Logic.Interfaces;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class DownloaderMapperTests {

    [TestMethod]
    public void Downloader_OverRustleLogs_DoesNotCrash() {
      var testContainerManager = new TestContainerManager();
      var downloadMapper = testContainerManager.Container.GetInstance<IDownloadMapper>();

      var logs = downloadMapper.OverRustleLogs("woopboop");

      Assert.IsNotNull(logs);
    }

  }
}
