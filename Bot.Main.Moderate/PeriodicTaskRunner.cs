using Bot.Pipeline.Interfaces;

namespace Bot.Main.Moderate {
  public class PeriodicTaskRunner {
    private readonly ICommandHandler _periodicMessages;
    private readonly ICommandHandler _periodicStreamStatusUpdater;
    private readonly ICommandHandler _periodicTwitterStatusUpdater;
    private readonly ICommandHandler _periodicClientChecker;

    public PeriodicTaskRunner(
      ICommandHandler periodicMessages,
      ICommandHandler periodicStreamStatusUpdater,
      ICommandHandler periodicTwitterStatusUpdater,
      ICommandHandler periodicClientChecker
    ) {
      _periodicMessages = periodicMessages;
      _periodicStreamStatusUpdater = periodicStreamStatusUpdater;
      _periodicTwitterStatusUpdater = periodicTwitterStatusUpdater;
      _periodicClientChecker = periodicClientChecker;
    }

    public void Run() {
      _periodicMessages.Handle();
      _periodicStreamStatusUpdater.Handle();
      _periodicClientChecker.Handle();
      _periodicTwitterStatusUpdater.Handle();
    }

  }
}
