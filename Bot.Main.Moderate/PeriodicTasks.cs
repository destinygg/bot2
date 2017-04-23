using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Models.Xml;
using Bot.Pipeline.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Main.Moderate {
  public class PeriodicTasks {
    private readonly ICommandHandler<IEnumerable<ISendable<ITransmittable>>> _sender;
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly IGenericClassFactory<string, string, string> _urlXmlParser;
    private readonly ISettings _settings;
    private readonly ILogger _logger;

    public PeriodicTasks(ICommandHandler<IEnumerable<ISendable<ITransmittable>>> sender, IQueryCommandService<IUnitOfWork> unitOfWork, IGenericClassFactory<string, string, string> urlXmlParser, ISettings settings, ILogger logger) {
      _sender = sender;
      _unitOfWork = unitOfWork;
      _urlXmlParser = urlXmlParser;
      _settings = settings;
      _logger = logger;
    }

    public void Run() {
      var periodicTaskFactory = new PeriodicTaskFactory();
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
      var feed = _urlXmlParser.Create<YoutubeFeed.Feed>("https://www.youtube.com/feeds/videos.xml?user=destiny", "", "");
      var video = feed?.Entry.OrderByDescending(x => x.ParsedPublished).First();
      return video == null
        ? "An error occured while contacting YouTube."
        : $"\"{video.Title}\" posted {(DateTime.UtcNow - video.ParsedPublished).ToPretty(_logger)} ago youtu.be/{video.VideoId}";
    }

  }
}
