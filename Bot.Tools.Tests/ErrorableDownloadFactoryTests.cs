﻿using System;
using System.Linq;
using System.Net;
using Bot.Main.Moderate;
using Bot.Pipeline.Tests;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SimpleInjector;

namespace Bot.Tools.Tests {
  [TestClass]
  public class ErrorableDownloadFactoryTests {

    private static TestContainerManager _createTestContainerManager(TestableLogger testableLogger, ITimeService timeService = null, Action<TestSettings> setSettings = null) {
      var downloadFactory = Substitute.For<IFactory<string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>()).Throws(_ => new WebException());
      return new TestContainerManager(c => {
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, c);
        if (timeService != null) {
          c.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
        }
        var downloadFactoryRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IFactory<string, string, string>), downloadFactoryRegistration, _ => true);
        var loggerRegistration = Lifestyle.Singleton.CreateRegistration(() => testableLogger, c);
        c.RegisterConditional(typeof(ILogger), loggerRegistration, pc => !pc.Handled);
      }, settings => setSettings?.Invoke(settings));
    }

    [TestMethod]
    public void ErrorableDownloadFactory_Real404_ReturnsErrorText_DoNotRunContinuously() {
      var testContainerManager = new TestContainerManager();
      var errorableDownloadFactory = testContainerManager.Container.GetInstance<IErrorableFactory<string, string, string, string>>();
      var errorText = Guid.NewGuid().ToString();

      var html = errorableDownloadFactory.Create("https://httpbin.org/404", "", errorText);

      Assert.AreEqual(errorText, html);
    }

    [TestMethod]
    public void ErrorableDownloadFactory_Fake404_ReturnsErrorTextNoErrorLogsAndOneWarningLog() {
      var testableLogger = new TestableLogger();
      var testContainerManager = _createTestContainerManager(testableLogger);
      var errorableDownloadFactory = testContainerManager.Container.GetInstance<IErrorableFactory<string, string, string, string>>();
      var errorText = Guid.NewGuid().ToString();

      var html = errorableDownloadFactory.Create("", "", errorText);

      testableLogger.ErrorOutbox.WriteDump();
      Assert.AreEqual(errorText, html);
      Assert.AreEqual(0, testableLogger.ErrorOutbox.Count);
      Assert.AreEqual(1, testableLogger.WarningOutbox.Count);
    }

    [TestMethod]
    public void ErrorableDownloadFactory_Fake404UntilMeetsErrorLimit_HasNoErrorLogsButSomeWarningLogs() {
      var testableLogger = new TestableLogger();
      var testContainerManager = _createTestContainerManager(testableLogger);
      var errorableDownloadFactory = testContainerManager.Container.GetInstance<IErrorableFactory<string, string, string, string>>();
      var settings = testContainerManager.Container.GetInstance<ISettings>();

      foreach (var i in Enumerable.Range(0, settings.DownloadErrorLimit)) {
        errorableDownloadFactory.Create("google.com", "", "");
      }

      Assert.AreEqual(0, testableLogger.ErrorOutbox.Count);
      Assert.AreNotEqual(0, testableLogger.WarningOutbox.Count);
    }

    [TestMethod]
    public void ErrorableDownloadFactory_Fake404UntilExceedsErrorLimit_Has1ErrorLog() {
      var testableLogger = new TestableLogger();
      var testContainerManager = _createTestContainerManager(testableLogger);
      var errorableDownloadFactory = testContainerManager.Container.GetInstance<IErrorableFactory<string, string, string, string>>();
      var settings = testContainerManager.Container.GetInstance<ISettings>();

      foreach (var i in Enumerable.Range(0, settings.DownloadErrorLimit + 1)) {
        errorableDownloadFactory.Create("google.com", "", "");
      }

      Assert.AreEqual(1, testableLogger.ErrorOutbox.Count);
    }

    [TestMethod]
    public void ErrorableDownloadFactory_Fake404OutOfWindow_Has0ErrorLogsAnd3InfoLogs() {
      var testableLogger = new TestableLogger();
      var timeService = Substitute.For<ITimeService>();
      var window = TimeSpan.FromHours(1);
      var time0 = DateTime.Today;
      var time1 = time0 + window + TimeSpan.FromTicks(1);
      timeService.UtcNow.Returns(time0, time1);
      var testContainerManager = _createTestContainerManager(testableLogger, timeService, s => {
        s.DownloadErrorLimit = 2;
        s.DownloadErrorWindow = window;
      });
      var errorableDownloadFactory = testContainerManager.Container.GetInstance<IErrorableFactory<string, string, string, string>>();
      var settings = testContainerManager.Container.GetInstance<ISettings>();

      foreach (var i in Enumerable.Range(0, settings.DownloadErrorLimit + 1)) {
        errorableDownloadFactory.Create("google.com", "", "");
      }

      Assert.AreEqual(0, testableLogger.ErrorOutbox.Count);
      Assert.AreEqual(3, testableLogger.WarningOutbox.Count);
    }

    [TestMethod]
    public void ErrorableDownloadFactory_Fake404InsideWindow_Has1ErrorLogAndTwoWarningLogs() {
      var testableLogger = new TestableLogger();
      var timeService = Substitute.For<ITimeService>();
      var window = TimeSpan.FromHours(1);
      var time0 = DateTime.Today;
      timeService.UtcNow.Returns(time0);
      var testContainerManager = _createTestContainerManager(testableLogger, timeService, s => {
        s.DownloadErrorLimit = 2;
        s.DownloadErrorWindow = window;
      });
      var errorableDownloadFactory = testContainerManager.Container.GetInstance<IErrorableFactory<string, string, string, string>>();
      var settings = testContainerManager.Container.GetInstance<ISettings>();

      foreach (var i in Enumerable.Range(0, settings.DownloadErrorLimit + 1)) {
        errorableDownloadFactory.Create("google.com", "", "");
      }

      Assert.AreEqual(1, testableLogger.ErrorOutbox.Count);
      Assert.AreEqual(3, testableLogger.WarningOutbox.Count);
    }

    [TestMethod]
    public void ErrorableDownloadFactory_PlainCreate_Works_DoNotRunContinuously() {
      var testContainerManager = new TestContainerManager();
      var errorableDownloadFactory = testContainerManager.Container.GetInstance<IErrorableFactory<string, string, string, string>>();

      var html = errorableDownloadFactory.Create("https://httpbin.org/html", "", "");

      var expectedHtml = "<!DOCTYPE html>\n<html>\n  <head>\n  </head>\n  <body>\n      <h1>Herman Melville - Moby-Dick</h1>\n\n      <div>\n        <p>\n          Availing himself of the mild, summer-cool weather that now reigned in these latitudes, and in preparation for the peculiarly active pursuits shortly to be anticipated, Perth, the begrimed, blistered old blacksmith, had not removed his portable forge to the hold again, after concluding his contributory work for Ahab's leg, but still retained it on deck, fast lashed to ringbolts by the foremast; being now almost incessantly invoked by the headsmen, and harpooneers, and bowsmen to do some little job for them; altering, or repairing, or new shaping their various weapons and boat furniture. Often he would be surrounded by an eager circle, all waiting to be served; holding boat-spades, pike-heads, harpoons, and lances, and jealously watching his every sooty movement, as he toiled. Nevertheless, this old man's was a patient hammer wielded by a patient arm. No murmur, no impatience, no petulance did come from him. Silent, slow, and solemn; bowing over still further his chronically broken back, he toiled away, as if toil were life itself, and the heavy beating of his hammer the heavy beating of his heart. And so it was.—Most miserable! A peculiar walk in this old man, a certain slight but painful appearing yawing in his gait, had at an early period of the voyage excited the curiosity of the mariners. And to the importunity of their persisted questionings he had finally given in; and so it came to pass that every one now knew the shameful story of his wretched fate. Belated, and not innocently, one bitter winter's midnight, on the road running between two country towns, the blacksmith half-stupidly felt the deadly numbness stealing over him, and sought refuge in a leaning, dilapidated barn. The issue was, the loss of the extremities of both feet. Out of this revelation, part by part, at last came out the four acts of the gladness, and the one long, and as yet uncatastrophied fifth act of the grief of his life's drama. He was an old man, who, at the age of nearly sixty, had postponedly encountered that thing in sorrow's technicals called ruin. He had been an artisan of famed excellence, and with plenty to do; owned a house and garden; embraced a youthful, daughter-like, loving wife, and three blithe, ruddy children; every Sunday went to a cheerful-looking church, planted in a grove. But one night, under cover of darkness, and further concealed in a most cunning disguisement, a desperate burglar slid into his happy home, and robbed them all of everything. And darker yet to tell, the blacksmith himself did ignorantly conduct this burglar into his family's heart. It was the Bottle Conjuror! Upon the opening of that fatal cork, forth flew the fiend, and shrivelled up his home. Now, for prudent, most wise, and economic reasons, the blacksmith's shop was in the basement of his dwelling, but with a separate entrance to it; so that always had the young and loving healthy wife listened with no unhappy nervousness, but with vigorous pleasure, to the stout ringing of her young-armed old husband's hammer; whose reverberations, muffled by passing through the floors and walls, came up to her, not unsweetly, in her nursery; and so, to stout Labor's iron lullaby, the blacksmith's infants were rocked to slumber. Oh, woe on woe! Oh, Death, why canst thou not sometimes be timely? Hadst thou taken this old blacksmith to thyself ere his full ruin came upon him, then had the young widow had a delicious grief, and her orphans a truly venerable, legendary sire to dream of in their after years; and all of them a care-killing competency.\n        </p>\n      </div>\n  </body>\n</html>";
      Assert.AreEqual(expectedHtml, html);
    }

    [TestMethod]
    public void ErrorableDownloadFactory_CreateWithHeader_Works_DoNotRunContinuously() {
      var testContainerManager = new TestContainerManager();
      var errorableDownloadFactory = testContainerManager.Container.GetInstance<IErrorableFactory<string, string, string, string>>();
      var key = "Key";
      var value = TestHelper.RandomString();
      var header = key + ":" + value;

      var html = errorableDownloadFactory.Create("https://httpbin.org/headers", header, "");

      var expectedHtml = "\"" + key + "\": \"" + value + "\"";
      Assert.IsTrue(html.Contains(expectedHtml));
    }

  }
}
