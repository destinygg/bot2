using System;
using System.Collections.Generic;
using Bot.Tools;
using Newtonsoft.Json;

namespace Bot.Models.Json {
  public class LastFm {
    public class Artist {
      [JsonProperty("#text")]
      public string text { get; set; }
      public string mbid { get; set; }
    }

    public class Album {
      [JsonProperty("#text")]
      public string text { get; set; }
      public string mbid { get; set; }
    }

    public class Image {
      [JsonProperty("#text")]
      public string text { get; set; }
      public string size { get; set; }
    }

    public class SongAttr {
      public bool nowplaying { get; set; }
    }

    public class Date {
      public string uts { get; set; }

      public DateTime Parsed_uts {
        get {
          var utsLong = long.Parse(uts);
          return utsLong.FromUnixTime();
        }
      }

      [JsonProperty("#text")]
      public string text { get; set; }
    }

    public class Track {
      public Artist artist { get; set; }
      public string name { get; set; }
      public string streamable { get; set; }
      public string mbid { get; set; }
      public Album album { get; set; }
      public string url { get; set; }
      public List<Image> image { get; set; }
      [JsonProperty("@attr")]
      public SongAttr attr { get; set; }
      public bool NowPlaying => attr != null && attr.nowplaying;
      public Date date { get; set; }
    }

    public class Attr {
      public string user { get; set; }
      public string page { get; set; }
      public string perPage { get; set; }
      public string totalPages { get; set; }
      public string total { get; set; }
    }

    public class Recenttracks {
      public List<Track> track { get; set; }
      [JsonProperty("@attr")]
      public Attr attr { get; set; }
    }

    public class RootObject {
      public Recenttracks recenttracks { get; set; }
    }
  }
}
