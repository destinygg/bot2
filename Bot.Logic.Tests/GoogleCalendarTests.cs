using System;
using Bot.Logic.Interfaces;
using Bot.Tests;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class GoogleCalendarTests {

    private static TestContainerManager TestContainerManager(string data, DateTime time) {
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(data);
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(time);
      var testContainerManager = new TestContainerManager(c => {
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, c);
        c.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
        c.RegisterConditional<IGenericClassFactory<string, string, string>, UrlJsonParser>(Lifestyle.Singleton, _ => true);
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
      });
      return testContainerManager;
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsSubDayInLength_BeforeFirst() {
      var time = new DateTime(2017, 5, 5);
      var data = TestData.GoogleCalendarFirstTenAreSubDayInLength;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"Nathan Times\" scheduled to begin in 19h 30m";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsSubDayInLength_DuringFirst() {
      var time = new DateTime(2017, 5, 5, 19, 30, 0);
      var data = TestData.GoogleCalendarFirstTenAreSubDayInLength;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"Nathan Times\" scheduled to begin in 0m";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsSubDayInLength_AfterFirst() {
      var time = new DateTime(2017, 5, 5, 19, 30, 1);
      var data = TestData.GoogleCalendarFirstTenAreSubDayInLength;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"Stream - Factorio/PUBG\" scheduled to begin in 5h 29m";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsAllDayEventSecondIsWithinThatDay_BeforeFirst() {
      var time = new DateTime(2017, 5, 4, 0, 0, 0);
      var data = TestData.GoogleCalendarFirstIsAllDayEventSecondIsWithinThatDay;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"WILL HE MAKE THE YT VIDEO TODAY? FIND OUT NEXT TIME, ON DESTINYBALLZ\", an all day event, is scheduled to begin in 5h";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsAllDayEventSecondIsWithinThatDay_DuringFirst() {
      var time = new DateTime(2017, 5, 4, 5, 0, 0);
      var data = TestData.GoogleCalendarFirstIsAllDayEventSecondIsWithinThatDay;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"WILL HE MAKE THE YT VIDEO TODAY? FIND OUT NEXT TIME, ON DESTINYBALLZ\", an all day event, is scheduled to begin in 0m";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsAllDayEventSecondIsWithinThatDay_AfterFirst() {
      var time = new DateTime(2017, 5, 4, 5, 0, 1);
      var data = TestData.GoogleCalendarFirstIsAllDayEventSecondIsWithinThatDay;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"WILL HE MAKE THE YT VIDEO TODAY? FIND OUT NEXT TIME, ON DESTINYBALLZ\", an all day event, is scheduled for today. \"Nathan Times\" scheduled to begin in 14h 29m";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsAllDayEventSecondIsAfterThatDay_BeforeFirst() {
      var time = new DateTime(2017, 5, 4, 0, 0, 0);
      var data = TestData.GoogleCalendarFirstIsAllDayEventSecondIsAfterThatDay;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"WILL HE MAKE THE YT VIDEO TODAY? FIND OUT NEXT TIME, ON DESTINYBALLZ\", an all day event, is scheduled to begin in 5h";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsAllDayEventSecondIsAfterThatDay_DuringFirst() {
      var time = new DateTime(2017, 5, 4, 5, 0, 0);
      var data = TestData.GoogleCalendarFirstIsAllDayEventSecondIsAfterThatDay;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"WILL HE MAKE THE YT VIDEO TODAY? FIND OUT NEXT TIME, ON DESTINYBALLZ\", an all day event, is scheduled to begin in 0m";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstIsAllDayEventSecondIsAfterThatDay_AfterFirst() {
      var time = new DateTime(2017, 5, 4, 5, 0, 1);
      var data = TestData.GoogleCalendarFirstIsAllDayEventSecondIsAfterThatDay;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"WILL HE MAKE THE YT VIDEO TODAY? FIND OUT NEXT TIME, ON DESTINYBALLZ\", an all day event, is scheduled for today. \"Stream - Chrono Trigger\" scheduled to begin in 1 day 11h";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstTwoAreMultiDayEvents_BeforeFirst() {
      var time = new DateTime(2017, 5, 28, 0, 0, 0);
      var data = TestData.GoogleCalendarFirstTwoAreMultiDayEvents;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"In Taiwan\", an all day event, is scheduled to begin in 5h";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstTwoAreMultiDayEvents_DuringFirst() {
      var time = new DateTime(2017, 5, 28, 5, 0, 0);
      var data = TestData.GoogleCalendarFirstTwoAreMultiDayEvents;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"In Taiwan\", an all day event, is scheduled to begin in 0m";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GoogleCalendar_FirstTwoAreMultiDayEvents_AfterFirst() {
      var time = new DateTime(2017, 5, 28, 5, 0, 1);
      var data = TestData.GoogleCalendarFirstTwoAreMultiDayEvents;
      var testContainerManager = TestContainerManager(data, time);
      var commandLogic = testContainerManager.Container.GetInstance<ICommandLogic>();
      var expected = "\"In Taiwan\", an all day event, is scheduled for today. \"Paying Bills, Monthly Schedule\" scheduled to begin in 3 days 23h";

      var actual = commandLogic.Schedule().Transmission.Text;

      Assert.AreEqual(expected, actual);
    }

  }
}
