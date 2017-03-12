using System;
using System.Runtime.CompilerServices;
using Bot.Tests;
using Bot.Tools;
using NSubstitute;
using SimpleInjector;

namespace Bot.Database.Tests {
  internal static class DatabaseHelper {

    public static Container GetContainerWithRecreatedAndIsolatedDatabase([CallerMemberName] string sqlitePath = null) {

      var settings = Substitute.For<ISettings>();
      settings.SqlitePath.Returns(sqlitePath);
      Console.WriteLine("Database path is: " + sqlitePath);

      var containerManager = new TestContainerManager(
        container => {
          var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => settings, container);
          container.RegisterConditional(typeof(ISettings), settingsServiceRegistration, pc => !pc.Handled);
        });

      containerManager.Container.GetInstance<DatabaseInitializer>().Recreate();

      return containerManager.Container;
    }

  }
}
