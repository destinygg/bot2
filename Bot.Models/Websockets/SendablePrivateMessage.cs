using Newtonsoft.Json;

namespace Bot.Models.Websockets {
  public class SendablePrivateMessage : IDggJson {
    public SendablePrivateMessage(string input, string targetNick) {
      Data = input;
      Nick = targetNick;
    }

    public string Data { get; }

    public string Nick { get; }

    [JsonIgnore]
    public string Command => "PRIVMSG";
  }
}
