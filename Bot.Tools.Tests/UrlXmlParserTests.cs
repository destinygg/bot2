using Bot.Models.Xml;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Tools.Tests {
  [TestClass]
  [Ignore]
  public class UrlXmlParserTests {

    [TestMethod]
    public void UrlXmlParser_Youtube_Works() {
      var testContainerManager = new TestContainerManager(container => {
        container.RegisterConditional<IGenericClassFactory<string, string, string>, UrlXmlParser>(Lifestyle.Singleton, c => true);
      });
      var urlXmlParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();

      var feed = urlXmlParser.Create<YoutubeFeed.Feed>("https://www.youtube.com/feeds/videos.xml?user=destiny", "", "Error");

      Assert.AreEqual("Destiny", feed.Title);
    }

  }
}
