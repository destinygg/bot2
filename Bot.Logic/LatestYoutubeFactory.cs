using System;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class LatestYoutubeFactory : IFactory<string> {
    private readonly ILogger _logger;
    private readonly IDownloadMapper _downloadMapper;

    public LatestYoutubeFactory(
      ILogger logger,
      IDownloadMapper downloadMapper
    ) {
      _logger = logger;
      _downloadMapper = downloadMapper;
    }

    public string Create() {
      var feed = _downloadMapper.YoutubeFeed();
      var video = feed?.Entry.OrderByDescending(x => x.ParsedPublished).First();
      return video == null
        ? "An error occured while contacting YouTube."
        : $"\"{video.Title}\" posted {(DateTime.UtcNow - video.ParsedPublished).ToPretty(_logger)} ago youtu.be/{video.VideoId}";
    }

  }
}
