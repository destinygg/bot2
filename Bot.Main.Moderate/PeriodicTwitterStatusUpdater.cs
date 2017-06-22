using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using CoreTweet;

namespace Bot.Main.Moderate {
  public class PeriodicTwitterStatusUpdater : ICommandHandler {
    private readonly IFactory<Status, string, IEnumerable<string>> _twitterStatusFormatter;
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly IPipelineManager _pipelineManager;
    private readonly ITwitterManager _twitterManager;
    private readonly ISettings _settings;

    public PeriodicTwitterStatusUpdater(
      IFactory<Status, string, IEnumerable<string>> twitterStatusFormatter,
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      IQueryCommandService<IUnitOfWork> unitOfWork,
      IPipelineManager pipelineManager,
      ITwitterManager twitterManager,
      ISettings settings
    ) {
      _twitterStatusFormatter = twitterStatusFormatter;
      _periodicTaskFactory = periodicTaskFactory;
      _unitOfWork = unitOfWork;
      _pipelineManager = pipelineManager;
      _twitterManager = twitterManager;
      _settings = settings;
    }


    public void Handle() {
      _periodicTaskFactory.Create(_settings.TwitterStatusUpdaterInterval, () => {
        var latestDestinyTweetIdFromDb = _unitOfWork.Query(u => u.StateIntegers.LatestDestinyTweetId);
        var status = _twitterManager.LatestTweetFromDestiny();
        if (latestDestinyTweetIdFromDb != status.Id) {
          var formatted = _twitterStatusFormatter.Create(status, $"twitter.com/{status.User.ScreenName} just tweeted: ");
          var messages = formatted.Select(f => new SendablePublicMessage(f)).ToList();
          messages.ForEach(m => _pipelineManager.Enqueue(m));
        }
      });
    }

  }
}
