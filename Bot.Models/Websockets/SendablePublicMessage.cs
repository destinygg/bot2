using Newtonsoft.Json;

namespace Bot.Models.Websockets {
  public class SendablePublicMessage : IDggJson {
    public SendablePublicMessage(string input) {
      Data = input;
    }

    public string Data { get; set; }

    [JsonIgnore]
    public string Command => "MSG";
  }
}
