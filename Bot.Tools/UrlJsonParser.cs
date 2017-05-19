using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {

  public class UrlJsonParser : IGenericClassFactory<string, string, string> {
    private readonly IErrorableFactory<string, string, string, string> _errorableDownloadFactory;
    private readonly IGenericClassFactory<string> _jsonParser;

    public UrlJsonParser(IErrorableFactory<string, string, string, string> errorableDownloadFactory, IGenericClassFactory<string> jsonParser) {
      _errorableDownloadFactory = errorableDownloadFactory;
      _jsonParser = jsonParser;
    }

    public TResult Create<TResult>(string url, string header, string error)
      where TResult : class {
      var uniqueError = error + Guid.NewGuid();
      var json = _errorableDownloadFactory.Create(url, header, uniqueError);
      return json == uniqueError ? null : _jsonParser.Create<TResult>(json);
    }

  }
}
