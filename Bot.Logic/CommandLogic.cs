using System;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class CommandLogic : ICommandLogic {
    private readonly ITimeService _timeService;
    private readonly IDownloader _downloader;
    private readonly ILogger _logger;

    public CommandLogic(ITimeService timeService, IDownloader downloader, ILogger logger) {
      _timeService = timeService;
      _downloader = downloader;
      _logger = logger;
    }

    public ISendable<PublicMessage> Time() => new SendablePublicMessage($"{_timeService.DestinyNow.ToShortTimeString()} Central Steven Time");

    public ISendable<PublicMessage> Schedule() {
      var events = _downloader.GoogleCalendar().ExtendedItem;
      var nextEvent = events.First(e => e.ParsedStart >= _timeService.UtcNow);
      var nextString = $"\"{nextEvent.Item.summary}\" scheduled to begin in {(nextEvent.ParsedStart - _timeService.UtcNow).ToPretty(_logger)}";
      var first = events[0];
      if (first.Item.start.date == null) return new SendablePublicMessage(nextString);
      var delta = first.ParsedStart - _timeService.UtcNow;
      var scheduledString = delta < TimeSpan.Zero
        ? $"for today. {nextString}"
        : $"to begin in {delta.ToPretty(_logger)}";
      return new SendablePublicMessage($"\"{first.Item.summary}\", an all day event, is scheduled {scheduledString}");
    }

  }
}
