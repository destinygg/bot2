using Bot.Pipeline.Interfaces;

namespace Bot.Main.Moderate {
  public class PeriodicTaskRunner {
    private readonly ICommandHandler _periodicMessages;
    private readonly ICommandHandler _periodicStreamStatusUpdater;

    public PeriodicTaskRunner(
      ICommandHandler periodicMessages,
      ICommandHandler periodicStreamStatusUpdater) {
      _periodicMessages = periodicMessages;
      _periodicStreamStatusUpdater = periodicStreamStatusUpdater;
    }

    public void Run() {
      _periodicMessages.Handle();
      _periodicStreamStatusUpdater.Handle();
    }

  }
}
