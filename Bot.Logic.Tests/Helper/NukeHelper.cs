using System;
using Bot.Tests;
using Bot.Tools;
using Bot.Tools.Interfaces;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests.Helper {
  public static class NukeHelper {
    private static ISettings BuildSettings(TimeSpan nukeBlastRadius) {
      var settings = Substitute.For<ISettings>();
      settings.NukeBlastRadius.Returns(nukeBlastRadius);
      settings.NukeMinimumStringSimilarity.Returns(0.7);
      return settings;
    }

    private static ISettings BuildSettings(string nukeBlastRadius) => BuildSettings(TimeSpan.Parse(nukeBlastRadius));

    private static ITimeService BuildTimeService(DateTime now) {
      var timeService = Substitute.For<ITimeService>();
      timeService.UtcNow.Returns(now);
      return timeService;
    }

    private static ITimeService BuildTimeService(string now) => BuildTimeService(TestHelper.Parse(now));

    public static Container GetContainer(string now, ISettings settings) => GetContainer(BuildTimeService(now), settings);
    public static Container GetContainer(DateTime now, ISettings settings) => GetContainer(BuildTimeService(now), settings);
    public static Container GetContainer(ITimeService timeService, TimeSpan nukeBlastRadius) => GetContainer(timeService, BuildSettings(nukeBlastRadius));
    public static Container GetContainer(ITimeService timeService, string nukeBlastRadius) => GetContainer(timeService, BuildSettings(nukeBlastRadius));
    public static Container GetContainer(DateTime now, TimeSpan nukeBlastRadius) => GetContainer(BuildTimeService(now), BuildSettings(nukeBlastRadius));
    public static Container GetContainer(DateTime now, string nukeBlastRadius) => GetContainer(BuildTimeService(now), BuildSettings(nukeBlastRadius));
    public static Container GetContainer(string timeService, string nukeBlastRadius) => GetContainer(timeService, BuildSettings(nukeBlastRadius));

    public static Container GetContainer(ITimeService timeService, ISettings settings) {
      var containerManager = new TestContainerManager(
        container => {
          var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => timeService, container);
          container.RegisterConditional(typeof(ITimeService), timeServiceRegistration, pc => !pc.Handled);
          var settingsServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => settings, container);
          container.RegisterConditional(typeof(ISettings), settingsServiceRegistration, pc => !pc.Handled);
        });
      return containerManager.Container;
    }
  }
}
