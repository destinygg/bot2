using System;
using System.Runtime.CompilerServices;
using Bot.Tests;
using Bot.Tools;
using NSubstitute;
using SimpleInjector;

namespace Bot.Repository.Tests {
  public static class RepositoryHelper {

    public static Container GetContainerWithInitializedAndIsolatedRepository(Action<ISettings> configureSettings = null, [CallerMemberName] string sqliteName = null) {

      var settings = Substitute.For<ISettings>();
      settings.SqlitePath.Returns($"{sqliteName}_{TestHelper.RandomInt()}_Bot.sqlite");
      configureSettings?.Invoke(settings);
      var containerManager = new TestContainerManager(
        container => {
          var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => settings, container);
          container.RegisterConditional(typeof(ISettings), settingsServiceRegistration, pc => !pc.Handled);
        });

      containerManager.Container.GetInstance<RepositoryInitializer>().RecreateWithMasterData();

      return containerManager.Container;
    }

  }
}
