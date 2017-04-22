using System;
using System.Net;
using System.Text;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class DownloadFactory : IErrorableFactory<string, string, Tuple<bool, string>> {

    public Tuple<bool, string> Create(string url, string header = "") {
      var client = new WebClient { Encoding = Encoding.UTF8 };
      if (header != "") {
        client.Headers = new WebHeaderCollection { header };
      }
      return Tuple.Create(true, client.DownloadString(url));
    }

    public Tuple<bool, string> OnErrorCreate => Tuple.Create(false, "");
  }
}
