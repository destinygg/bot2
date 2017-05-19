using System.Net;
using System.Text;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class DownloadFactory : IFactory<string, string, string> {

    public string Create(string url, string header) {
      using (var client = new WebClient { Encoding = Encoding.UTF8 }) {
        if (header != "") {
          client.Headers = new WebHeaderCollection { header };
        }
        return client.DownloadString(url);
      }
    }

  }
}
