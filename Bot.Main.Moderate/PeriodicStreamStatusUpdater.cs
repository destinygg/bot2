using System;
using System.Threading.Tasks;
using Bot.Logic.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Main.Moderate {
  public class PeriodicStreamStatusUpdater : ICommandHandler {
    private readonly IProvider<IStreamStateService> _streamStateServiceProvider;
    private readonly ISettings _settings;
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;

    public PeriodicStreamStatusUpdater(
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      IProvider<IStreamStateService> streamStateServiceProvider,
      ISettings settings) {
      _periodicTaskFactory = periodicTaskFactory;
      _streamStateServiceProvider = streamStateServiceProvider;
      _settings = settings;
    }

    public void Handle() {
      _periodicTaskFactory.Create(_settings.PeriodicTaskInterval, () => _streamStateServiceProvider.Get().Get());
    }

  }
}
