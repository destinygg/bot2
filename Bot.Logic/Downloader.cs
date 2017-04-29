using Bot.Logic.Interfaces;
using Bot.Models.Json;
using Bot.Models.Xml;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class Downloader : IDownloader {

    private readonly IGenericClassFactory<string, string, string> _urlJsonParser;
    private readonly IGenericClassFactory<string, string, string> _urlXmlParser;
    private readonly IPrivateConstants _privateConstants;

    public Downloader(IGenericClassFactory<string, string, string> urlJsonParser, IGenericClassFactory<string, string, string> urlXmlParser, IPrivateConstants privateConstants) {
      _urlJsonParser = urlJsonParser;
      _urlXmlParser = urlXmlParser;
      _privateConstants = privateConstants;
    }

    public TwitchStreamStatus.RootObject StreamStatus() =>
      _urlJsonParser.Create<TwitchStreamStatus.RootObject>("https://api.twitch.tv/kraken/streams/destiny", _privateConstants.TwitchClientId, "");

    public YoutubeFeed.Feed YoutubeFeed() =>
      _urlXmlParser.Create<YoutubeFeed.Feed>("https://www.youtube.com/feeds/videos.xml?user=destiny", "", "");
  }
}
