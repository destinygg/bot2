using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandRegexTest {
    [TestMethod]
    public void ModCommandRegex_nuke_this_AlwaysMatches() {
      var testPhrase = "!nuke this";
      var modCommandRegex = new ModCommandRegex();

      var nukeRegex = modCommandRegex.Nuke;

      Assert.IsTrue(nukeRegex.IsMatch(testPhrase));
    }

  }
}
