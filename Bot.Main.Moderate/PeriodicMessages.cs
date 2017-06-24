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
using Bot.Tools.Logging;

namespace Bot.Main.Moderate {
  public class PeriodicMessages : ICommandHandler {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly IDownloadMapper _downloadMapper;
    private readonly ISettings _settings;
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;
    private readonly IPipelineManager _pipelineManager;
    private readonly ILogger _logger;

    public PeriodicMessages(
      IQueryCommandService<IUnitOfWork> unitOfWork,
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      IDownloadMapper downloadMapper,
      ISettings settings,
      IPipelineManager pipelineManager,
      ILogger logger) {
      _periodicTaskFactory = periodicTaskFactory;
      _pipelineManager = pipelineManager;
      _unitOfWork = unitOfWork;
      _downloadMapper = downloadMapper;
      _settings = settings;
      _logger = logger;
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
      yield return _getLatestYoutube();
    }

    private string _getLatestYoutube() {
      var feed = _downloadMapper.YoutubeFeed();
      var video = feed?.Entry.OrderByDescending(x => x.ParsedPublished).First();
      return video == null
        ? "An error occured while contacting YouTube."
        : $"\"{video.Title}\" posted {(DateTime.UtcNow - video.ParsedPublished).ToPretty(_logger)} ago youtu.be/{video.VideoId}";
    }

  }
}
