using Newtonsoft.Json;

namespace Bot.Models.Websockets {
  public class SendablePardon : IDggJson {
    public SendablePardon(string nick) {
      Data = nick;
    }

    public string Data { get; }

    [JsonIgnore]
    public string Command => "UNBAN";
  }
}

/*
 
  case "unmute":
  case "unban":
    if (parts.length == 1) {
        this.gui.push(new ChatInfoMessage("Usage: /" + command + " nick"));
        return;
    }

    if (!nickregex.test(parts[1])) {
        this.gui.push(new ChatErrorMessage("Invalid nick - /" + command + " nick"));
        return;
    }

    payload.data = parts[1];
    this.emit(command.toUpperCase(), payload);
    break;

 */
