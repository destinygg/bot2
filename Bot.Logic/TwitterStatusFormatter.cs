using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Bot.Tools.Interfaces;
using CoreTweet;

namespace Bot.Logic {
  public class TwitterStatusFormatter : IFactory<Status, string, IEnumerable<string>> {

    public IEnumerable<string> Create(Status status, string prefix) {
      var text = status.RetweetedStatus == null ? status.FullText : $"RT @{status.RetweetedStatus.User.ScreenName}: {status.RetweetedStatus.FullText}";
      text = HttpUtility.HtmlDecode(text);
      text = prefix + text;
      if (status.RetweetedStatus?.Entities?.Media != null) {
        text = status.RetweetedStatus.Entities.Media.GroupBy(p => p.Url).ToDictionary(x => x.Key, y => y.First().DisplayUrl).Aggregate(text, _Replace);
      }
      if (status.RetweetedStatus?.Entities != null) {
        text = status.RetweetedStatus.Entities.Urls.GroupBy(p => p.Url).ToDictionary(x => x.Key, y => y.First().ExpandedUrl).Aggregate(text, _Replace);
      }
      if (status.Entities.Media != null) {
        text = status.Entities.Media.GroupBy(p => p.Url).ToDictionary(x => x.Key, y => y.First().DisplayUrl).Aggregate(text, _Replace);
      }
      text = status.Entities.Urls.GroupBy(p => p.Url).ToDictionary(x => x.Key, y => y.First().ExpandedUrl).Aggregate(text, _Replace);
      while (text.Contains("\n\n")) {
        text = text.Replace("\n\n", "\n");
      }
      return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
    }

    private string _Replace(string text, KeyValuePair<string, string> x) {
      if (string.IsNullOrWhiteSpace(x.Key) || string.IsNullOrWhiteSpace(x.Value)) {
        return text;
      }
      var strippedValue = Regex.Replace(x.Value, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase);
      return text.Replace(x.Key, strippedValue);
    }

  }
}
