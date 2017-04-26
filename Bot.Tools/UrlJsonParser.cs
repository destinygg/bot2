using System;
using System.IO;
using System.Text;
using Bot.Tools.Interfaces;
using Newtonsoft.Json;

namespace Bot.Tools {

  // http://json2csharp.com

  public class UrlJsonParser : IGenericClassFactory<string, string, string> {
    private readonly IErrorableFactory<string, string, string, string> _downloadFactory;

    public UrlJsonParser(IErrorableFactory<string, string, string, string> downloadFactory) {
      _downloadFactory = downloadFactory;
    }

    public TResult Create<TResult>(string url, string header, string error)
      where TResult : class {
      var uniqueError = error + Guid.NewGuid();
      var json = _downloadFactory.Create(url, header, uniqueError);
      if (json == uniqueError) return null;
      var byteArray = Encoding.UTF8.GetBytes(json);
      using (var stream = new MemoryStream(byteArray))
      using (var streamReader = new StreamReader(stream))
      using (var jsonTextReader = new JsonTextReader(streamReader)) {
        var serializer = new JsonSerializer();
        return serializer.Deserialize<TResult>(jsonTextReader);
      }
    }

  }
}
