using System.Linq;
using Bot.Models;
using Bot.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandLogicTests {
    [TestMethod]
    public void SimpleAegis() {
      //Arrange
      var contextBuilder = new ContextBuilder();
      var context = contextBuilder
        .PublicMessage("derp")
        .TargetedMessage("MESSAGE")
        .TargetedMessage("message")
        .TargetedMessage("message the quick brown fox jumped over the lazy dog")
        .PublicMessage("Innocent as well")
        .ModMessage("!nuke MESSage")
        .GetContext;

      var consoleLogger = new ConsoleLogger();
      var regex = new ModCommandRegex();
      var parser = new ModCommandParser(regex, consoleLogger);
      var logic = new ModCommandLogic(consoleLogger, regex, parser);

      //Act
      var aegisedUsers = logic.Aegis(context).Cast<SendableUnMuteBan>().Select(umb => umb.Target).ToList();

      //Assert
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

  }
}
