namespace Bot.Models.Websockets {
  public class SendablePardon {
    public SendablePardon(string nick) {
      Data = nick;
    }

    public string Data { get; }
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
