using System.Collections.Generic;

namespace Bot.Models.Websockets {
  public class ReceivedNames {

    public class User {
      public string nick { get; set; }
      public List<string> features { get; set; }
    }

    public class RootObject {
      public int connectioncount { get; set; }
      public List<User> users { get; set; }
    }

  }
}
