using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Main.Moderate.Tests {
  [TestClass]
  public class TestContainerManagerTests {

    [TestMethod]
    public void Verify_Never_ThrowsException() {
      var testContainerManager = new TestContainerManager();

      testContainerManager.Container.Verify();
    }

  }
}
