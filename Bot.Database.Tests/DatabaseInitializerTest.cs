using System;
using System.IO;
using Bot.Tests;
using Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Database.Tests {
  [TestClass]
  public class DatabaseInitializerTest {
    [TestMethod]
    public void EnsureCreated_Always_CreatesFile() {
      var path = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid() + ".sqlite");
      var settings = Substitute.For<ISettings>();
      settings.SqlitePath.Returns(path);
      Console.WriteLine(path);
      var containerManager = new TestContainerManager(
        container => {
          var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => settings, container);
          container.RegisterConditional(typeof(ISettings), settingsServiceRegistration, pc => !pc.Handled);
        });
      var databaseInitializer = containerManager.Container.GetInstance<DatabaseInitializer>();
      if (File.Exists(path))
        File.Delete(path);
      Assert.IsFalse(File.Exists(path));

      databaseInitializer.EnsureCreated();

      Assert.IsTrue(File.Exists(path));
    }

    [TestMethod]
    public void EnsureDeleted_Always_DeletesFile() {
      var path = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid() + ".sqlite");
      var settings = Substitute.For<ISettings>();
      settings.SqlitePath.Returns(path);
      Console.WriteLine(path);
      var containerManager = new TestContainerManager(
        container => {
          var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => settings, container);
          container.RegisterConditional(typeof(ISettings), settingsServiceRegistration, pc => !pc.Handled);
        });
      var databaseInitializer = containerManager.Container.GetInstance<DatabaseInitializer>();
      if (!File.Exists(path))
        databaseInitializer.EnsureCreated();
      Assert.IsTrue(File.Exists(path));

      databaseInitializer.EnsureDeleted();

      Assert.IsFalse(File.Exists(path));
    }

  }
}
