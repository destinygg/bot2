using Bot.Models.Xml;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Tools.Tests {
  [TestClass]
  public class UrlXmlParserTests {

    [TestMethod]
    public void UrlXmlParser_Youtube_Works() {
      var errorableDownloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      errorableDownloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(TestData.YoutubeFeed);
      var testContainerManager = new TestContainerManager(container => {
        container.RegisterConditional<IGenericClassFactory<string, string, string>, UrlXmlParser>(Lifestyle.Singleton, c => true);
        var errorableDownloadFactoryRegistration = Lifestyle.Singleton.CreateRegistration(() => errorableDownloadFactory, container);
        container.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), errorableDownloadFactoryRegistration, _ => true);
      });
      var urlXmlParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();

      var feed = urlXmlParser.Create<YoutubeFeed.Feed>("", "", "");

      Assert.AreEqual("Destiny", feed.Title);
    }

  }
}
