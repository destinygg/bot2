using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class ErrorableDownloadFactory : IErrorableFactory<string, string, string, string> {
    private readonly IFactory<string, string, string> _downloadFactory;

    public ErrorableDownloadFactory(IFactory<string, string, string> downloadFactory) {
      _downloadFactory = downloadFactory;
    }

    public string Create(string url, string header, string error) {
      OnErrorCreate = error;
      return _downloadFactory.Create(url, header);
    }

    public string OnErrorCreate { get; private set; }
  }
}
