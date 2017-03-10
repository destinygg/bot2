using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Tests {
  [TestClass]
  public class TestContainerManagerTests {

    [TestMethod]
    public void Verify_Never_ThrowsException() {
      var testContainerManager = new TestContainerManager();

      testContainerManager.Container.Verify();
    }

  }
}
