using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace Bot.Models.Xml {
  public class YoutubeFeed {
    [XmlRoot(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
    public class Link {
      [XmlAttribute(AttributeName = "rel")]
      public string Rel { get; set; }
      [XmlAttribute(AttributeName = "href")]
      public string Href { get; set; }
    }

    [XmlRoot(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
    public class Author {
      [XmlElement(ElementName = "name", Namespace = "http://www.w3.org/2005/Atom")]
      public string Name { get; set; }
      [XmlElement(ElementName = "uri", Namespace = "http://www.w3.org/2005/Atom")]
      public string Uri { get; set; }
    }

    [XmlRoot(ElementName = "content", Namespace = "http://search.yahoo.com/mrss/")]
    public class Content {
      [XmlAttribute(AttributeName = "url")]
      public string Url { get; set; }
      [XmlAttribute(AttributeName = "type")]
      public string Type { get; set; }
      [XmlAttribute(AttributeName = "width")]
      public string Width { get; set; }
      [XmlAttribute(AttributeName = "height")]
      public string Height { get; set; }
    }

    [XmlRoot(ElementName = "thumbnail", Namespace = "http://search.yahoo.com/mrss/")]
    public class Thumbnail {
      [XmlAttribute(AttributeName = "url")]
      public string Url { get; set; }
      [XmlAttribute(AttributeName = "width")]
      public string Width { get; set; }
      [XmlAttribute(AttributeName = "height")]
      public string Height { get; set; }
    }

    [XmlRoot(ElementName = "starRating", Namespace = "http://search.yahoo.com/mrss/")]
    public class StarRating {
      [XmlAttribute(AttributeName = "count")]
      public string Count { get; set; }
      [XmlAttribute(AttributeName = "average")]
      public string Average { get; set; }
      [XmlAttribute(AttributeName = "min")]
      public string Min { get; set; }
      [XmlAttribute(AttributeName = "max")]
      public string Max { get; set; }
    }

    [XmlRoot(ElementName = "statistics", Namespace = "http://search.yahoo.com/mrss/")]
    public class Statistics {
      [XmlAttribute(AttributeName = "views")]
      public string Views { get; set; }
    }

    [XmlRoot(ElementName = "community", Namespace = "http://search.yahoo.com/mrss/")]
    public class Community {
      [XmlElement(ElementName = "starRating", Namespace = "http://search.yahoo.com/mrss/")]
      public StarRating StarRating { get; set; }
      [XmlElement(ElementName = "statistics", Namespace = "http://search.yahoo.com/mrss/")]
      public Statistics Statistics { get; set; }
    }

    [XmlRoot(ElementName = "group", Namespace = "http://search.yahoo.com/mrss/")]
    public class Group {
      [XmlElement(ElementName = "title", Namespace = "http://search.yahoo.com/mrss/")]
      public string Title { get; set; }
      [XmlElement(ElementName = "content", Namespace = "http://search.yahoo.com/mrss/")]
      public Content Content { get; set; }
      [XmlElement(ElementName = "thumbnail", Namespace = "http://search.yahoo.com/mrss/")]
      public Thumbnail Thumbnail { get; set; }
      [XmlElement(ElementName = "description", Namespace = "http://search.yahoo.com/mrss/")]
      public string Description { get; set; }
      [XmlElement(ElementName = "community", Namespace = "http://search.yahoo.com/mrss/")]
      public Community Community { get; set; }
    }

    [XmlRoot(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
    public class Entry {
      [XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
      public string Id { get; set; }
      [XmlElement(ElementName = "videoId", Namespace = "http://www.youtube.com/xml/schemas/2015")]
      public string VideoId { get; set; }
      [XmlElement(ElementName = "channelId", Namespace = "http://www.youtube.com/xml/schemas/2015")]
      public string ChannelId { get; set; }
      [XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
      public string Title { get; set; }
      [XmlElement(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
      public Link Link { get; set; }
      [XmlElement(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
      public Author Author { get; set; }
      [XmlElement(ElementName = "published", Namespace = "http://www.w3.org/2005/Atom")]
      public string Published { get; set; }
      [XmlIgnore]
      public DateTime ParsedPublished => DateTime.ParseExact(Published, "yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture).ToUniversalTime();
      [XmlElement(ElementName = "updated", Namespace = "http://www.w3.org/2005/Atom")]
      public string Updated { get; set; }
      [XmlElement(ElementName = "group", Namespace = "http://search.yahoo.com/mrss/")]
      public Group Group { get; set; }
    }

    [XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class Feed {
      [XmlElement(ElementName = "script", Namespace = "http://www.w3.org/2005/Atom")]
      public string Script { get; set; }
      [XmlElement(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
      public List<Link> Link { get; set; }
      [XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
      public string Id { get; set; }
      [XmlElement(ElementName = "channelId", Namespace = "http://www.youtube.com/xml/schemas/2015")]
      public string ChannelId { get; set; }
      [XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
      public string Title { get; set; }
      [XmlElement(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
      public Author Author { get; set; }
      [XmlElement(ElementName = "published", Namespace = "http://www.w3.org/2005/Atom")]
      public string Published { get; set; }
      [XmlElement(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
      public List<Entry> Entry { get; set; }
      [XmlAttribute(AttributeName = "yt", Namespace = "http://www.w3.org/2000/xmlns/")]
      public string Yt { get; set; }
      [XmlAttribute(AttributeName = "media", Namespace = "http://www.w3.org/2000/xmlns/")]
      public string Media { get; set; }
      [XmlAttribute(AttributeName = "xmlns")]
      public string Xmlns { get; set; }
      [XmlAttribute(AttributeName = "class")]
      public string Class { get; set; }
    }
  }
}
