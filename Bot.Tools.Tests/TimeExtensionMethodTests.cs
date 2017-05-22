using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Tools.Tests {

  [TestClass]
  public class TimeExtensionMethodTests {

    [TestMethod]
    public void TimeExtensionMethod_ToUnixTime_year2k() {
      var year2K = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      var expected = 946684800L;

      var actual = year2K.ToUnixTime();

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TimeExtensionMethod_FromUnixTime_year2k() {
      var year2K = 946684800L;
      var expected = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

      var actual = year2K.FromUnixTime();

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TimeExtensionMethod_PrettyDeltaTime_HasPrettyOutput() {
      var testList = new List<TimeSpan> {
        new TimeSpan(days:50,hours:23,minutes:59,seconds:59),

        new TimeSpan(days:6,hours:23,minutes:59,seconds:59),
        new TimeSpan(days:6,hours:1,minutes:0,seconds:0),
        new TimeSpan(days:6,hours:0,minutes:0,seconds:0),

        new TimeSpan(days:1,hours:23,minutes:0,seconds:0),
        new TimeSpan(days:1,hours:1,minutes:0,seconds:0),
        new TimeSpan(days:1,hours:0,minutes:0,seconds:0),

        new TimeSpan(hours:23,minutes:59,seconds:0),
        new TimeSpan(hours:23,minutes:1,seconds:0),
        new TimeSpan(hours:23,minutes:0,seconds:0),

        new TimeSpan(hours:1,minutes:59,seconds:0),
        new TimeSpan(hours:1,minutes:1,seconds:0),
        new TimeSpan(hours:1,minutes:0,seconds:0),

        new TimeSpan(hours:0,minutes:59,seconds:59),
        new TimeSpan(hours:0,minutes:59,seconds:1),
        new TimeSpan(hours:0,minutes:59,seconds:0),

        new TimeSpan(hours:0,minutes:1,seconds:59),
        new TimeSpan(hours:0,minutes:1,seconds:1),
        new TimeSpan(hours:0,minutes:1,seconds:0),

        new TimeSpan(hours:0,minutes:0,seconds:59),
        new TimeSpan(hours:0,minutes:0,seconds:1),
        new TimeSpan(hours:0,minutes:0,seconds:0),

      };

      var actualAnswer = new List<string>();

      var expectedAnswer = new List<string> {
        "50 days 23h",

        "6 days 23h",
        "6 days 1h",
        "6 days",

        "1 day 23h",
        "1 day 1h",
        "1 day",

        "23h 59m",
        "23h 1m",
        "23h",

        "1h 59m",
        "1h 1m",
        "1h",

        "59m",
        "59m",
        "59m",

        "1m",
        "1m",
        "1m",

        "0m",
        "0m",
        "0m",
      };

      foreach (var x in testList.Select((ts, i) => new { ts, i })) {
        actualAnswer.Add(x.ts.ToPretty(null));
        Assert.AreEqual(expectedAnswer[x.i], actualAnswer[x.i]);
      }

      CollectionAssert.AreEqual(expectedAnswer, actualAnswer);
    }
  }
}
