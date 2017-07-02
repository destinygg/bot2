using System;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Main.Moderate {
  public class PeriodicTwitterStatusUpdater : ICommandHandler {
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly IPipelineManager _pipelineManager;
    private readonly ITwitterManager _twitterManager;
    private readonly ISettings _settings;

    public PeriodicTwitterStatusUpdater(
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      IQueryCommandService<IUnitOfWork> unitOfWork,
      IPipelineManager pipelineManager,
      ITwitterManager twitterManager,
      ISettings settings
    ) {
      _periodicTaskFactory = periodicTaskFactory;
      _unitOfWork = unitOfWork;
      _pipelineManager = pipelineManager;
      _twitterManager = twitterManager;
      _settings = settings;
    }

    public void Handle() {
      _periodicTaskFactory.Create(_settings.TwitterStatusUpdaterInterval, () => {
        var latestDestinyTweetIdFromDb = _unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
        var formattedStatus = _twitterManager.LatestTweetFromDestiny(true);
        var formatted = formattedStatus.Item1;
        var status = formattedStatus.Item2;
        if (latestDestinyTweetIdFromDb != status.Id) {
          var messages = formatted.Select(f => new SendablePublicMessage(f)).ToList();
          messages.ForEach(m => _pipelineManager.Enqueue(m));
        }
      });
    }

  }
}
