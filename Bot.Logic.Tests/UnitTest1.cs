using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class UnitTest1 {
    [TestMethod]
    public void TestMethod1() {
      var mc = new ModCommandRegex();
      var nuke = mc.Nuke;
      Assert.IsTrue(nuke.IsMatch("!nuke this"));
      var s = nuke.Match("!nuke this").Groups[1];
    }
  }
}
