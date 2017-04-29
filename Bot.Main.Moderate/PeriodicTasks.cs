using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Main.Moderate {
  public class PeriodicTasks {
    private readonly ICommandHandler<IEnumerable<ISendable<ITransmittable>>> _sender;
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly IDownloader _downloader;
    private readonly ISettings _settings;
    private readonly ILogger _logger;

    public PeriodicTasks(
      ICommandHandler<IEnumerable<ISendable<ITransmittable>>> sender,
      IQueryCommandService<IUnitOfWork> unitOfWork,
      IDownloader downloader,
      ISettings settings,
      ILogger logger) {
      _sender = sender;
      _unitOfWork = unitOfWork;
      _downloader = downloader;
      _settings = settings;
      _logger = logger;
    }

    public void Run() {
      var periodicTaskFactory = new PeriodicTaskFactory();
      RepeatingMessages(periodicTaskFactory);
    }

    private void RepeatingMessages(PeriodicTaskFactory periodicTaskFactory) {
      var rng = new Random();
      var messageCount = PeriodicMessages().Count();
      var i = rng.Next(messageCount);
      periodicTaskFactory.Create(_settings.PeriodicTaskInterval, () => {
        _sender.Handle(new SendablePublicMessage(PeriodicMessages().Skip(i).First()).Wrap());
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

  }
}
