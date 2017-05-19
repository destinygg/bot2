using Bot.Models.Json;
using Bot.Models.Xml;

namespace Bot.Logic.Interfaces {
  public interface IDownloadMapper {
    TwitchStreamStatus.RootObject StreamStatus();
    YoutubeFeed.Feed YoutubeFeed();
    GoogleCalendar.RootObject GoogleCalendar();
    DestinyGgBlogFeed.Rss DestinyGgBlogFeed();
    string OverRustle();
    LastFm.RootObject LastFm();
    string OverRustleLogs(string nick);
  }
}
