using System.IO;
using System.Text;
using Bot.Tools.Interfaces;
using Newtonsoft.Json;

namespace Bot.Tools {
  public class JsonParser : IGenericClassFactory<string> {

    public TResult Create<TResult>(string json) where TResult : class {
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
