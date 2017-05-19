using System;
using Newtonsoft.Json;

namespace Bot.Models.Websockets {
  public class SendableBan : IDggJson {
    public SendableBan(string victim, bool isIpBan, TimeSpan duration, bool isPermanent, string reason) {
      Nick = victim;
      BanIp = isIpBan;
      Reason = reason;
      Duration = ((ulong) duration.TotalMilliseconds) * 1000000UL;
      IsPermanent = isPermanent;
    }

    public string Nick { get; set; }
    public ulong Duration { get; set; }
    public bool BanIp { get; set; }
    public bool IsPermanent { get; set; }
    public string Reason { get; set; }

    [JsonIgnore]
    public string Command => "BAN";

    public bool ShouldSerializeIsPermanent() => IsPermanent;

    public bool ShouldSerializeBanIp() => BanIp;

    public bool ShouldSerializeDuration() => !IsPermanent;
  }
}

/*
case "ban":
case "ipban":
    if (parts.length < 4) {
        this.gui.push(new ChatInfoMessage("Usage: /" + command + " nick time reason (time can be 'permanent')"));
        return;
    }

    if (!nickregex.test(parts[1])) {
        this.gui.push(new ChatErrorMessage("Invalid nick"));
        return;
    }

    payload.nick = parts[1];
    if (command == "ipban")
        payload.banip = true;

    if (/^perm/i.test(parts[2]))
        payload.ispermanent = true;
    else
        payload.duration = this.parseTimeInterval(parts[2]);

    payload.reason = parts.slice(3, parts.length).join(' ');
    if (!payload.reason) {
        this.gui.push(new ChatErrorMessage("Providing a reason is mandatory"));
        return;
    }

    this.emit("BAN", payload);
    break;
*/
