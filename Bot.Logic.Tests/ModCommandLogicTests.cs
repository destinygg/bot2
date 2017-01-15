using System.Linq;
using Bot.Models;
using Bot.Pipeline.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

      var logger = new Mock<ILogger>().Object;
      var regex = new ModCommandRegex();
      var parser = new ModCommandParser(regex, logger);
      var logic = new ModCommandLogic(logger, regex, parser);

      //Act
      var aegis = logic.Aegis(context);

      //Assert
      var aegisedUsers = aegis.Cast<SendableUnMuteBan>().Select(umb => umb.Target).ToList();
      Assert.IsTrue(contextBuilder.IsValid(aegisedUsers));
    }

  }
}
