using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Main.Moderate {
  public class PeriodicMessages : ICommandHandler {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly ISettings _settings;
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;
    private readonly IPipelineManager _pipelineManager;
    private readonly IFactory<string> _latestYoutubeFactory;

    public PeriodicMessages(
      IQueryCommandService<IUnitOfWork> unitOfWork,
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      ISettings settings,
      IPipelineManager pipelineManager,
      IFactory<string> latestYoutubeFactory
    ) {
      _periodicTaskFactory = periodicTaskFactory;
      _pipelineManager = pipelineManager;
      _latestYoutubeFactory = latestYoutubeFactory;
      _unitOfWork = unitOfWork;
      _settings = settings;
    }


    public void Handle() {
      var rng = new Random();
      var messageCount = _getMessages().Count();
      var i = rng.Next(messageCount);
      _periodicTaskFactory.Create(_settings.PeriodicMessageInterval, () => {
        _pipelineManager.Enqueue(new SendablePublicMessage(_getMessages().Skip(i).First()));
        i++;
        if (i >= messageCount) {
          i = 0;
        }
      });
    }

    private IEnumerable<string> _getMessages() {
      foreach (var sendablePublicMessage in _unitOfWork.Query(u => u.PeriodicMessages.GetAll)) {
        yield return sendablePublicMessage;
      }
      yield return _latestYoutubeFactory.Create();
    }

  }
}
