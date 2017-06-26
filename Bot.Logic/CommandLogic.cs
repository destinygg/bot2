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
    private readonly IDownloadMapper _downloadMapper;
    private readonly ILogger _logger;
    private readonly ITwitterManager _twitterManager;
    private readonly IProvider<IStreamStateService> _streamStateServiceProvider;
    private readonly ISettings _settings;
    private readonly IFactory<string> _latestYoutubeFactory;

    public CommandLogic(
      ITimeService timeService,
      IDownloadMapper downloadMapper,
      ILogger logger,
      ITwitterManager twitterManager,
      IProvider<IStreamStateService> streamStateServiceProvider,
      ISettings settings,
      IFactory<string> latestYoutubeFactory
    ) {
      _timeService = timeService;
      _downloadMapper = downloadMapper;
      _logger = logger;
      _twitterManager = twitterManager;
      _streamStateServiceProvider = streamStateServiceProvider;
      _settings = settings;
      _latestYoutubeFactory = latestYoutubeFactory;
    }

    public ISendable<PublicMessage> Time() => new SendablePublicMessage($"{_timeService.DestinyNow.ToShortTimeString()} Central Steven Time");

    public ISendable<PublicMessage> Schedule() => new SendablePublicMessage(_rawSchedule() + " destiny.gg/schedule");

    private string _rawSchedule() {
      var events = _downloadMapper.GoogleCalendar().ExtendedItem;
      var nextEvent = events.First(e => e.ParsedStart >= _timeService.UtcNow);
      var nextString = $"\"{nextEvent.Item.summary}\" scheduled to begin in {(nextEvent.ParsedStart - _timeService.UtcNow).ToPretty(_logger)}";
      var first = events[0];
      if (first.Item.start.date == null) return nextString;
      var delta = first.ParsedStart - _timeService.UtcNow;
      var scheduledString = delta < TimeSpan.Zero
        ? $"for today. {nextString}"
        : $"to begin in {delta.ToPretty(_logger)}";
      return $"\"{first.Item.summary}\", an all day event, is scheduled {scheduledString}";
    }

    public ISendable<PublicMessage> Blog() {
      var firstEntry = _downloadMapper.DestinyGgBlogFeed().Channel.Item[0];
      return new SendablePublicMessage($"\"{firstEntry.Title}\" posted {(_timeService.UtcNow - firstEntry.Parsed_PubDate).ToPretty(_logger)} ago {firstEntry.Link2}");
    }

    public IEnumerable<ISendable<PublicMessage>> Streams() {
      dynamic overrustle = JsonConvert.DeserializeObject(_downloadMapper.OverRustle());
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

    public IEnumerable<ISendable<PublicMessage>> TwitterDestiny() => _twitterManager.LatestTweetFromDestiny("").Item1.Select(x => new SendablePublicMessage(x));

    public IEnumerable<ISendable<PublicMessage>> TwitterAslan() => _twitterManager.LatestTweetFromAslan("").Select(x => new SendablePublicMessage(x));

    public IEnumerable<ISendable<PublicMessage>> Song() {
      var song = _downloadMapper.LastFm().recenttracks.track.First();
      var songString = $"{song.name} - {song.artist.text}";
      string response;
      if (song.NowPlaying) {
        response = $"{songString} last.fm/user/stevenbonnellii";
      } else {
        var delta = (_timeService.UtcNow - song.date.Parsed_uts).ToPretty(_logger);
        response = $"No song played/scrobbled. Played {delta} ago: {songString}";
      }
      return new SendablePublicMessage(response).Wrap();
    }

    public IEnumerable<ISendable<PublicMessage>> PreviousSong() {
      var first = _downloadMapper.LastFm().recenttracks.track.First();
      var second = _downloadMapper.LastFm().recenttracks.track.Skip(1).First();
      var firstString = $"{first.name} - {first.artist.text}";
      var secondString = $"{second.name} - {second.artist.text}";
      var delta = (_timeService.UtcNow - second.date.Parsed_uts).ToPretty(_logger);
      var response = $"{secondString} played {delta} ago before {firstString}";
      return new SendablePublicMessage(response).Wrap();
    }

    public IEnumerable<ISendable<PublicMessage>> Live() {
      var status = _streamStateServiceProvider.Get().Get();
      switch (status.StreamStatus) {
        case StreamStatus.On: {
            var delta = _timeService.UtcNow - status.LatestStreamOnTime;
            return new SendablePublicMessage($"Live with {status.Viewers} viewers for {delta.ToPretty(_logger)} playing {status.Game}: {status.Title}").Wrap();
          }
        case StreamStatus.Off: {
            var delta = _timeService.UtcNow - status.LatestStreamOffTime;
            var streamLength = status.LatestStreamOffTime - status.LatestStreamOnTime;
            return new SendablePublicMessage($"Stream went offline {delta.ToPretty(_logger)} ago and its duration was {streamLength.ToPretty(_logger)}").Wrap();
          }
        case StreamStatus.PossiblyOff: {
            var streamLength = status.LatestStreamOffTime - status.LatestStreamOnTime;
            return new SendablePublicMessage($"Stream went offline in the past ~{_settings.OnOffTimeTolerance.ToPretty(_logger)} and its duration was {streamLength.ToPretty(_logger)}").Wrap();
          }
        default:
          throw new NotSupportedException($"The stream status {status.StreamStatus} is not registered");
      }
    }

    public IEnumerable<ISendable<PublicMessage>> Youtube() => new SendablePublicMessage(_latestYoutubeFactory.Create()).Wrap();

  }
}
