using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {

  public class UrlJsonParser : IGenericClassFactory<string, string, string> {
    private readonly IErrorableFactory<string, string, string, string> _downloadFactory;
    private readonly IGenericClassFactory<string> _jsonParser;

    public UrlJsonParser(IErrorableFactory<string, string, string, string> downloadFactory, IGenericClassFactory<string> jsonParser) {
      _downloadFactory = downloadFactory;
      _jsonParser = jsonParser;
    }

    public TResult Create<TResult>(string url, string header, string error)
      where TResult : class {
      var uniqueError = error + Guid.NewGuid();
      var json = _downloadFactory.Create(url, header, uniqueError);
      return json == uniqueError ? null : _jsonParser.Create<TResult>(json);
    }

  }
}
