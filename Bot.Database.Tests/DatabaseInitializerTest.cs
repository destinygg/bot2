using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  [TestClass]
  public class DatabaseInitializerTest {
    [TestMethod]
    public void EnsureCreated_Always_CreatesFile() {
      var path = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid() + ".sqlite");
      var databaseInitializer = DatabaseHelper.GetContainerWithInitializedAndIsolatedDatabase(path).GetInstance<DatabaseInitializer>();
      if (File.Exists(path))
        File.Delete(path);
      Assert.IsFalse(File.Exists(path));

      databaseInitializer.EnsureCreated();

      Assert.IsTrue(File.Exists(path));
    }

    [TestMethod]
    public void EnsureDeleted_Always_DeletesFile() {
      var path = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid() + ".sqlite");
      var databaseInitializer = DatabaseHelper.GetContainerWithInitializedAndIsolatedDatabase(path).GetInstance<DatabaseInitializer>();
      if (!File.Exists(path))
        databaseInitializer.EnsureCreated();
      Assert.IsTrue(File.Exists(path));

      databaseInitializer.EnsureDeleted();

      Assert.IsFalse(File.Exists(path));
    }

  }
}
