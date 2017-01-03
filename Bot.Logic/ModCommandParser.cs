using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bot.Logic.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Logic {
  public class ModCommandParser : IModCommandParser {
    private readonly IModRegex _modRegex;
    private readonly ILogger _logger;

    public ModCommandParser(IModRegex modRegex, ILogger logger) {
      _modRegex = modRegex;
      _logger = logger;
    }

    public string Stalk(string input) => _firstGroup(_modRegex.Stalk, input);
    public bool SubOnly(string input) => _firstGroup(_modRegex.SubOnly, input) == "on";

    public Tuple<string, string> AddCommand(string input) => _stringStringGroupsToTuple(_modRegex.Mute, input);
    public string DelCommand(string input) => _firstGroup(_modRegex.DelCommand, input);

    public Tuple<string, TimeSpan> AddMute(string input) => _numberUnitStringGroupsToTuple(_modRegex.Mute, input);
    public Tuple<string, TimeSpan> AddBan(string input) => _numberUnitStringGroupsToTuple(_modRegex.AddBan, input);
    public Tuple<string, TimeSpan> AddMuteRegex(string input) => _numberUnitStringGroupsToTuple(_modRegex.Mute, input);
    public Tuple<string, TimeSpan> AddBanRegex(string input) => _numberUnitStringGroupsToTuple(_modRegex.Mute, input);

    public string DelMute(string input) => _firstGroup(_modRegex.DelMute, input);
    public string DelBan(string input) => _firstGroup(_modRegex.DelBan, input);
    public string DelMuteRegex(string input) => _firstGroup(_modRegex.DelMuteRegex, input);
    public string DelBanRegex(string input) => _firstGroup(_modRegex.DelBanRegex, input);

    public Tuple<string, TimeSpan> Mute(string input) => _numberUnitStringGroupsToTuple(_modRegex.Mute, input);
    public Tuple<string, TimeSpan> Ban(string input) => _numberUnitStringGroupsToTuple(_modRegex.Ban, input);
    public Tuple<string, TimeSpan> Ipban(string input) => _numberUnitStringGroupsToTuple(_modRegex.Ipban, input);
    public string UnMuteBan(string input) => _firstGroup(_modRegex.UnMuteBan, input);

    public Tuple<string, TimeSpan> Nuke(string input) => _numberUnitStringGroupsToTuple(_modRegex.Nuke, input);
    public Tuple<string, TimeSpan> RegexNuke(string input) => _numberUnitStringGroupsToTuple(_modRegex.RegexNuke, input);

    private string _firstGroup(Regex regex, string input) => regex.Match(input).Groups[1].Value;

    private Tuple<string, string> _stringStringGroupsToTuple(Regex regex, string input) {
      var gc = regex.Match(input).Groups;
      var string1 = gc[1].Value;
      var string2 = gc[2].Value;
      return Tuple.Create(string1, string2);
    }

    private Tuple<string, TimeSpan> _numberUnitStringGroupsToTuple(Regex regex, string input) {
      var gc = regex.Match(input).Groups;
      var number = gc[1].Value;
      var unit = gc[2].Value;
      var target = gc[3].Value;
      return Tuple.Create(target, _toTimespan(number, unit));
    }

    private TimeSpan _toTimespan(string stringInt, string s, bool ip = false) {
      int i;
      try {
        i = stringInt == "" ? 10 : int.Parse(stringInt);
      } catch (OverflowException) {
        i = int.MaxValue;
      }

      if (_modRegex.Seconds.Any(x => x == s)) {
        return TimeSpan.FromSeconds(i);
      }
      if (_modRegex.Minutes.Any(x => x == s)) {
        return TimeSpan.FromMinutes(i);
      }
      if (_modRegex.Hours.Any(x => x == s)) {
        return TimeSpan.FromHours(i);
      }
      if (_modRegex.Days.Any(x => x == s)) {
        return TimeSpan.FromDays(i);
      }
      if (_modRegex.Perm.Any(x => x == s)) {
        return TimeSpan.Zero;
      }
      if (s == "") {
        if (ip && stringInt == "") return TimeSpan.Zero;
        return TimeSpan.FromMinutes(i);
      }
      _logger.LogError($"Somehow an invalid time passed the regex. StringInt:{stringInt}, s:{s}, ip:{ip}");
      return TimeSpan.FromMinutes(10);
    }

  }
}
