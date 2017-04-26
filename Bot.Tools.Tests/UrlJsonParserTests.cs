using System;
using Bot.Models.Xml;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Tools.Tests {
  [TestClass]
  public class UrlJsonParserTests {

    [TestMethod]
    public void UrlJsonParser_TwitchLiveStatus_ViewersWorks() {
      var rawTwitchJson = @"{""stream"":{""_id"":25130128624,""game"":""Minecraft"",""viewers"":2557,""video_height"":1080,""average_fps"":62.1030345801,""delay"":0,""created_at"":""2017-04-24T21:23:16Z"",""is_playlist"":false,""preview"":{""small"":""https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-80x45.jpg"",""medium"":""https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-320x180.jpg"",""large"":""https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-640x360.jpg"",""template"":""https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-{width}x{height}.jpg""},""channel"":{""mature"":true,""partner"":true,""status"":""memes"",""broadcaster_language"":""en"",""display_name"":""Destiny"",""game"":""Minecraft"",""language"":""en"",""_id"":18074328,""name"":""destiny"",""created_at"":""2010-11-22T04:14:56Z"",""updated_at"":""2017-04-25T01:07:43Z"",""delay"":null,""logo"":""https://static-cdn.jtvnw.net/jtv_user_pictures/destiny-profile_image-951fd53950bc2f8b-300x300.png"",""banner"":null,""video_banner"":""https://static-cdn.jtvnw.net/jtv_user_pictures/destiny-channel_offline_image-9f84d937a04358a9-1920x1080.jpeg"",""background"":null,""profile_banner"":null,""profile_banner_background_color"":null,""url"":""https://www.twitch.tv/destiny"",""views"":72173621,""followers"":165848,""_links"":{""self"":""https://api.twitch.tv/kraken/channels/destiny"",""follows"":""https://api.twitch.tv/kraken/channels/destiny/follows"",""commercial"":""https://api.twitch.tv/kraken/channels/destiny/commercial"",""stream_key"":""https://api.twitch.tv/kraken/channels/destiny/stream_key"",""chat"":""https://api.twitch.tv/kraken/chat/destiny"",""features"":""https://api.twitch.tv/kraken/channels/destiny/features"",""subscriptions"":""https://api.twitch.tv/kraken/channels/destiny/subscriptions"",""editors"":""https://api.twitch.tv/kraken/channels/destiny/editors"",""teams"":""https://api.twitch.tv/kraken/channels/destiny/teams"",""videos"":""https://api.twitch.tv/kraken/channels/destiny/videos""}},""_links"":{""self"":""https://api.twitch.tv/kraken/streams/destiny""}},""_links"":{""self"":""https://api.twitch.tv/kraken/streams/destiny"",""channel"":""https://api.twitch.tv/kraken/channels/destiny""}}";
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(rawTwitchJson);
      var testContainerManager = new TestContainerManager(c => {
        c.RegisterConditional<IGenericClassFactory<string, string, string>, UrlJsonParser>(Lifestyle.Singleton, _ => true);
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
      });
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();

      var twitchLiveStatus = urlJsonParser.Create<TwitchLiveStatus.RootObject>("", "", "");

      Assert.AreEqual(2557, twitchLiveStatus.stream.viewers);
    }

    [TestMethod]
    public void UrlJsonParser_TwitchLiveStatus_ParsedCreatedAtWorks() {
      var rawTwitchJson = @"{""stream"":{""_id"":25130128624,""game"":""Minecraft"",""viewers"":2557,""video_height"":1080,""average_fps"":62.1030345801,""delay"":0,""created_at"":""2017-04-24T21:23:16Z"",""is_playlist"":false,""preview"":{""small"":""https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-80x45.jpg"",""medium"":""https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-320x180.jpg"",""large"":""https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-640x360.jpg"",""template"":""https://static-cdn.jtvnw.net/previews-ttv/live_user_destiny-{width}x{height}.jpg""},""channel"":{""mature"":true,""partner"":true,""status"":""memes"",""broadcaster_language"":""en"",""display_name"":""Destiny"",""game"":""Minecraft"",""language"":""en"",""_id"":18074328,""name"":""destiny"",""created_at"":""2010-11-22T04:14:56Z"",""updated_at"":""2017-04-25T01:07:43Z"",""delay"":null,""logo"":""https://static-cdn.jtvnw.net/jtv_user_pictures/destiny-profile_image-951fd53950bc2f8b-300x300.png"",""banner"":null,""video_banner"":""https://static-cdn.jtvnw.net/jtv_user_pictures/destiny-channel_offline_image-9f84d937a04358a9-1920x1080.jpeg"",""background"":null,""profile_banner"":null,""profile_banner_background_color"":null,""url"":""https://www.twitch.tv/destiny"",""views"":72173621,""followers"":165848,""_links"":{""self"":""https://api.twitch.tv/kraken/channels/destiny"",""follows"":""https://api.twitch.tv/kraken/channels/destiny/follows"",""commercial"":""https://api.twitch.tv/kraken/channels/destiny/commercial"",""stream_key"":""https://api.twitch.tv/kraken/channels/destiny/stream_key"",""chat"":""https://api.twitch.tv/kraken/chat/destiny"",""features"":""https://api.twitch.tv/kraken/channels/destiny/features"",""subscriptions"":""https://api.twitch.tv/kraken/channels/destiny/subscriptions"",""editors"":""https://api.twitch.tv/kraken/channels/destiny/editors"",""teams"":""https://api.twitch.tv/kraken/channels/destiny/teams"",""videos"":""https://api.twitch.tv/kraken/channels/destiny/videos""}},""_links"":{""self"":""https://api.twitch.tv/kraken/streams/destiny""}},""_links"":{""self"":""https://api.twitch.tv/kraken/streams/destiny"",""channel"":""https://api.twitch.tv/kraken/channels/destiny""}}";
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(rawTwitchJson);
      var testContainerManager = new TestContainerManager(c => {
        c.RegisterConditional<IGenericClassFactory<string, string, string>, UrlJsonParser>(Lifestyle.Singleton, _ => true);
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
      });
      var urlJsonParser = testContainerManager.Container.GetInstance<IGenericClassFactory<string, string, string>>();

      var parsedCreatedAt = urlJsonParser.Create<TwitchLiveStatus.RootObject>("", "", "").stream.Parsed_created_at;

      Assert.AreEqual(new DateTime(2017, 04, 24, 21, 23, 16, DateTimeKind.Utc), parsedCreatedAt);
    }

  }
}
