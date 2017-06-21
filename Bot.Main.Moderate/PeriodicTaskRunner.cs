using Bot.Pipeline.Interfaces;

namespace Bot.Main.Moderate {
  public class PeriodicTaskRunner {
    private readonly ICommandHandler _periodicMessages;
    private readonly ICommandHandler _periodicStreamStatusUpdater;
    private readonly ICommandHandler _periodicClientChecker;

    public PeriodicTaskRunner(
      ICommandHandler periodicMessages,
      ICommandHandler periodicStreamStatusUpdater,
      ICommandHandler periodicClientChecker
    ) {
      _periodicMessages = periodicMessages;
      _periodicStreamStatusUpdater = periodicStreamStatusUpdater;
      _periodicClientChecker = periodicClientChecker;
    }

    public void Run() {
      _periodicMessages.Handle();
      _periodicStreamStatusUpdater.Handle();
      _periodicClientChecker.Handle();
    }

  }
}
