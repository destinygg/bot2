using System;
using System.Threading.Tasks;
using Bot.Logic.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Main.Moderate {
  public class PeriodicTaskRunner {
    private readonly IProvider<IStreamStateService> _streamStateServiceProvider;
    private readonly ICommandHandler _periodicMessages;
    private readonly ISettings _settings;
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;

    public PeriodicTaskRunner(
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      IProvider<IStreamStateService> streamStateServiceProvider,
      ICommandHandler periodicMessages,
      ISettings settings) {
      _periodicTaskFactory = periodicTaskFactory;
      _streamStateServiceProvider = streamStateServiceProvider;
      _periodicMessages = periodicMessages;
      _settings = settings;
    }

    public void Run() {
      _periodicMessages.Handle();
      RefreshStreamStatus(_periodicTaskFactory);
    }

    private void RefreshStreamStatus(IFactory<TimeSpan, Action, Task> periodicTaskFactory) =>
      periodicTaskFactory.Create(_settings.PeriodicTaskInterval, () => _streamStateServiceProvider.Get().Get());

  }
}
