using Bot.Models.Json;
using Bot.Models.Xml;

namespace Bot.Logic.Interfaces {
  public interface IDownloader {
    TwitchStreamStatus.RootObject StreamStatus();
    YoutubeFeed.Feed YoutubeFeed();
    GoogleCalendar.RootObject GoogleCalendar();
  }
}
