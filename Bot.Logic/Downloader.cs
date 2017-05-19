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
    private readonly IErrorableFactory<string, string, string, string> _downloadFactory;
    private readonly IPrivateConstants _privateConstants;
    private readonly ITimeService _timeService;

    public Downloader(IGenericClassFactory<string, string, string> urlJsonParser, IGenericClassFactory<string, string, string> urlXmlParser, IErrorableFactory<string, string, string, string> downloadFactory, IPrivateConstants privateConstants, ITimeService timeService) {
      _urlJsonParser = urlJsonParser;
      _urlXmlParser = urlXmlParser;
      _downloadFactory = downloadFactory;
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

    public DestinyGgBlogFeed.Rss DestinyGgBlogFeed() => _urlXmlParser.Create<DestinyGgBlogFeed.Rss>($"http://blog.destiny.gg/feed/", "", "");

    public string OverRustle() => _downloadFactory.Create("http://api.overrustle.com/api", "", "");

    public LastFm.RootObject LastFm() => _urlJsonParser.Create<LastFm.RootObject>($"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=stevenbonnellii&api_key={_privateConstants.LastFmApiKey}&format=json", "", "");

    public string OverRustleLogs(string user) => _downloadFactory.Create($"https://dgg.overrustlelogs.net/Destinygg%20chatlog/current/{user}.txt", "", "");
  }
}
