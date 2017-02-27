using System.IO;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  [TestClass]
  public class DatabaseInitializerTest {
    [TestMethod]
    public void EnsureCreated_Always_CreatesFile() {
      var cm = new TestContainerManager();
      var databaseInitializer = cm.Container.GetInstance<DatabaseInitializer>();
      var path = "Bot.db";
      if (File.Exists(path))
        File.Delete(path);
      Assert.IsFalse(File.Exists(path));

      databaseInitializer.EnsureCreated();

      Assert.IsTrue(File.Exists(path));
    }

    [TestMethod]
    public void EnsureDeleted_Always_DeletesFile() {
      var cm = new TestContainerManager();
      var databaseInitializer = cm.Container.GetInstance<DatabaseInitializer>();
      var path = "Bot.db";
      if (!File.Exists(path))
        databaseInitializer.EnsureCreated();
      Assert.IsTrue(File.Exists(path));

      databaseInitializer.EnsureDeleted();

      Assert.IsFalse(File.Exists(path));
    }

  }
}
