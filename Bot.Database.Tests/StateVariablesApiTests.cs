﻿using System;
using Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {

  [TestClass]
  public class StateVariablesApiTests {

    [TestMethod]
    public void GetAndSetOnTime() {
      // Arrange
      var factory = new ApiFactory();
      var api = factory.GetStateVariablesApi;
      var expected = DateTime.UtcNow;

      // Act
      api.SetOnTime(expected);
      var actual = api.GetOnTime();

      // Assert
      Assert.AreEqual(expected.ToUnixTime(), actual.ToUnixTime()); // Need Unix time to round; 

    }
  }
}