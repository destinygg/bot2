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
  public class PeriodicTasks {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly IStreamStateService _streamStateService;
    private readonly IDownloader _downloader;
    private readonly ISettings _settings;
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;
    private readonly IPipeline _pipeline;
    private readonly ILogger _logger;

    public PeriodicTasks(
      IQueryCommandService<IUnitOfWork> unitOfWork,
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      IStreamStateService streamStateService,
      IDownloader downloader,
      ISettings settings,
      IPipeline pipeline,
      ILogger logger) {
      _periodicTaskFactory = periodicTaskFactory;
      _pipeline = pipeline;
      _unitOfWork = unitOfWork;
      _streamStateService = streamStateService;
      _downloader = downloader;
      _settings = settings;
      _logger = logger;
    }

    public void Run() {
      RepeatingMessages(_periodicTaskFactory);
      RefreshStreamStatus(_periodicTaskFactory);
    }

    private void RepeatingMessages(IFactory<TimeSpan, Action, Task> periodicTaskFactory) {
      var rng = new Random();
      var messageCount = PeriodicMessages().Count();
      var i = rng.Next(messageCount);
      periodicTaskFactory.Create(_settings.PeriodicTaskInterval, () => {
        _pipeline.Enqueue(new SendablePublicMessage(PeriodicMessages().Skip(i).First()));
        i++;
        if (i >= messageCount) {
          i = 0;
        }
      });
    }

    private IEnumerable<string> PeriodicMessages() {
      foreach (var sendablePublicMessage in _unitOfWork.Query(u => u.PeriodicMessages.GetAll)) {
        yield return sendablePublicMessage;
      }
      yield return GetLatestYoutube();
    }

    private string GetLatestYoutube() {
      var feed = _downloader.YoutubeFeed();
      var video = feed?.Entry.OrderByDescending(x => x.ParsedPublished).First();
      return video == null
        ? "An error occured while contacting YouTube."
        : $"\"{video.Title}\" posted {(DateTime.UtcNow - video.ParsedPublished).ToPretty(_logger)} ago youtu.be/{video.VideoId}";
    }

    private void RefreshStreamStatus(IFactory<TimeSpan, Action, Task> periodicTaskFactory) =>
      periodicTaskFactory.Create(_settings.PeriodicTaskInterval, () => _streamStateService.Get());

  }
}
