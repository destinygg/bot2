using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bot.Logic.Contracts;

namespace Bot.Logic {
  public class ModCommandRegex : IModCommandRegex {

    public Regex Sing => _compiledIgnoreCase(@"^!sing.*");
    public Regex Dance => _compiledIgnoreCase(@"^!dance.*");

    public Regex Stalk => _compiledIgnoreCase(@"^!stalk (.+)");
    public Regex SubOnly => _compiledIgnoreCase(@"^!(?:sub.*) (on|off)$");

    public Regex AddCommand => _compiledIgnoreCase(@"^!addcommand !(\S+) (.+)");
    public Regex DelCommand => _compiledIgnoreCase(@"^!del(?:ete)?command !?(.+)");

    public Regex AddMute => _generate("add|addmute", true, false);
    public Regex AddBan => _generate("addban", true, false);
    public Regex AddMuteRegex => _generate("addregex|addmuteregex", true, false);
    public Regex AddBanRegex => _generate("addbanregex", true, false);

    public Regex ListMute => _compiledIgnoreCase(@"^!listmute.*");
    public Regex ListBan => _compiledIgnoreCase(@"^!listban.*");
    public Regex ListMuteRegex => _compiledIgnoreCase(@"^!listmuteregex.*");
    public Regex ListBanRegex => _compiledIgnoreCase(@"^!listbanregex.*");

    public Regex DelMute => _compiledIgnoreCase(@"^!del(?:ete)?(?:mute)? (.+)");
    public Regex DelBan => _compiledIgnoreCase(@"^!del(?:ete)?ban (.+)");
    public Regex DelMuteRegex => _compiledIgnoreCase(@"^!del(?:ete)?(?:mute)?regex (.+)");
    public Regex DelBanRegex => _compiledIgnoreCase(@"^!del(?:ete)?banregex (.+)");

    public Regex Mute => _generate("mute|m", true, true);
    public Regex Ban => _generate("ban|b", true, true);
    public Regex Ipban => _generate("ipban|ip|i", true, true);
    public Regex UnMuteBan => _compiledIgnoreCase(@"^!(?:unban|unmute) (.+)");

    public Regex Nuke => _generate("nuke|annihilate|obliterate", false, false);
    public Regex RegexNuke => _generate("regexnuke|regexpnuke|nukeregex|nukeregexp", false, false);
    public Regex Aegis => _compiledIgnoreCase(@"^!aegis.*");

    public IList<string> AllUnits => new List<string>().Concat(AllButPerm).Concat(Perm).ToList();
    public IList<string> AllButPerm => new List<string>().Concat(Seconds).Concat(Minutes).Concat(Hours).Concat(Days).ToList();
    public IList<string> Seconds => new List<string> { "s", "sec", "secs", "second", "seconds", };
    public IList<string> Minutes => new List<string> { "m", "min", "mins", "minute", "minutes", };
    public IList<string> Hours => new List<string> { "h", "hr", "hrs", "hour", "hours", };
    public IList<string> Days => new List<string> { "d", "day", "days", };
    public IList<string> Perm => new List<string> { "p", "per", "perm", "permanent" };

    private Regex _generate(string triggers, bool allowPerm, bool hasReason) {
      var times = allowPerm ? AllUnits : AllButPerm;
      var user = hasReason ? @" +(\S+) *" : " +";
      return _compiledIgnoreCase($"^!(?:{triggers}) *(?:(\\d*)({string.Join("|", times)})?)?{user}(.*)");
    }

    private Regex _compiledIgnoreCase(string pattern) => new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
  }
}
