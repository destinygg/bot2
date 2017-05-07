using System;
using Newtonsoft.Json;

namespace Bot.Models.Websockets {
  public class SendableMute : IDggJson {
    public SendableMute(string victim, TimeSpan duration) {
      Data = victim;
      Duration = ((ulong) duration.TotalMilliseconds) * 1000000UL;
    }
    public string Data { get; }
    public ulong Duration { get; }

    [JsonIgnore]
    public string Command => "MUTE";
  }
}

/*
   case "mute":
    if (parts.length == 1) {
        this.gui.push(new ChatInfoMessage("Usage: /" + command + " nick[ time]"));
        return;
    }

    // TODO bans are a little more involved, requiring a reason + ip bans + permbans
    if (!nickregex.test(parts[1])) {
        this.gui.push(new ChatErrorMessage("Invalid nick - /" + command + " nick[ time]"));
        return;
    }

    var duration = null;
    if (parts[2])
        duration = this.parseTimeInterval(parts[2])

    payload.data = parts[1];
    if (duration && duration > 0)
        payload.duration = duration;

    this.emit(command.toUpperCase(), payload);
    break;
*/
