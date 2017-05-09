using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    public ISendable<PublicMessage> Blog() {
      var firstEntry = _downloader.DestinyGgBlogFeed().Channel.Item[0];
      return new SendablePublicMessage($"\"{firstEntry.Title}\" posted {(_timeService.UtcNow - firstEntry.Parsed_PubDate).ToPretty(_logger)} ago {firstEntry.Link2}");
    }

    public IEnumerable<ISendable<PublicMessage>> Streams() {
      dynamic overrustle = JsonConvert.DeserializeObject(_downloader.OverRustle());
      var streamListArray = (JArray) overrustle.stream_list;
      foreach (var stream in streamListArray.Children().Take(3)) {
        var sb = new StringBuilder();
        foreach (dynamic detail in stream.Children()) {
          if (detail.Name == "rustlers") {
            sb.Append(detail.Value.Value);
          } else if (detail.Name == "url") {
            sb.Append($" overrustle.com{detail.Value.Value}");
            yield return new SendablePublicMessage(sb.ToString());
          }
        }
      }
    }

  }
}
