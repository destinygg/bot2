using System.Collections.Generic;

namespace Bot.Models.Websockets {
  public class ReceivedMsg {
    public class RootObject {
      public string nick { get; set; }
      public List<string> features { get; set; }
      public long timestamp { get; set; }
      public string data { get; set; }
    }
  }
}
