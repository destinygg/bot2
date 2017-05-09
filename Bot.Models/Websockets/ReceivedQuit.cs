using System.Collections.Generic;

namespace Bot.Models.Websockets {
  public class ReceivedQuit {
    public class RootObject {
      public string nick { get; set; }
      public List<string> features { get; set; }
      public long timestamp { get; set; }
    }
  }
}
