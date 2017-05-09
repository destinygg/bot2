using System.Net;
using System.Xml;
using Bot.Logic.Interfaces;
using Bot.Models.Json;
using Bot.Models.Xml;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class Downloader : IDownloader {

    private readonly IGenericClassFactory<string, string, string> _urlJsonParser;
    private readonly IGenericClassFactory<string, string, string> _urlXmlParser;
    private readonly IPrivateConstants _privateConstants;
    private readonly ITimeService _timeService;

    public Downloader(IGenericClassFactory<string, string, string> urlJsonParser, IGenericClassFactory<string, string, string> urlXmlParser, IPrivateConstants privateConstants, ITimeService timeService) {
      _urlJsonParser = urlJsonParser;
      _urlXmlParser = urlXmlParser;
      _privateConstants = privateConstants;
      _timeService = timeService;
    }

    public TwitchStreamStatus.RootObject StreamStatus() =>
      _urlJsonParser.Create<TwitchStreamStatus.RootObject>("https://api.twitch.tv/kraken/streams/destiny", _privateConstants.TwitchClientId, "");

    public YoutubeFeed.Feed YoutubeFeed() =>
      _urlXmlParser.Create<YoutubeFeed.Feed>("https://www.youtube.com/feeds/videos.xml?user=destiny", "", "");

    public GoogleCalendar.RootObject GoogleCalendar() {
      var xmlTime = XmlConvert.ToString(_timeService.UtcNow, XmlDateTimeSerializationMode.Utc);
      var time = WebUtility.HtmlEncode(xmlTime);
      return _urlJsonParser.Create<GoogleCalendar.RootObject>($"https://www.googleapis.com/calendar/v3/calendars/i54j4cu9pl4270asok3mqgdrhk%40group.calendar.google.com/events?orderBy=startTime&singleEvents=true&timeMin={time}&key={_privateConstants.GoogleKey}", "", "");
    }

    public DestinyGgBlogFeed.Rss DestinyGgBlogFeed() => _urlJsonParser.Create<DestinyGgBlogFeed.Rss>($"http://blog.destiny.gg/feed/", "", "");

  }
}
