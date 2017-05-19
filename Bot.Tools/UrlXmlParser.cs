using System;
using System.IO;
using System.Xml.Serialization;
using Bot.Tools.Interfaces;

namespace Bot.Tools {

  // http://xmltocsharp.azurewebsites.net/

  public class UrlXmlParser : IGenericClassFactory<string, string, string> {
    private readonly IErrorableFactory<string, string, string, string> _errorableDownloadFactory;

    public UrlXmlParser(IErrorableFactory<string, string, string, string> errorableDownloadFactory) {
      _errorableDownloadFactory = errorableDownloadFactory;
    }

    public TResult Create<TResult>(string url, string header, string error)
      where TResult : class {
      var uniqueError = error + Guid.NewGuid();
      var xml = _errorableDownloadFactory.Create(url, header, uniqueError);
      if (xml == uniqueError) return null;
      var serializer = new XmlSerializer(typeof(TResult));
      using (var reader = new StringReader(xml)) {
        return (TResult) serializer.Deserialize(reader);
      }
    }

  }
}
