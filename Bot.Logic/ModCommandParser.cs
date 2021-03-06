﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bot.Logic.Interfaces;

namespace Bot.Logic {
  public class ModCommandParser : IModCommandParser {
    private readonly IModCommandRegex _modCommandRegex;

    public ModCommandParser(IModCommandRegex modCommandRegex) {
      _modCommandRegex = modCommandRegex;
    }

    public string Stalk(string input) => _firstGroup(_modCommandRegex.Stalk, input);
    public bool SubOnly(string input) => _firstGroup(_modCommandRegex.SubOnly, input) == "on";

    public Tuple<string, string> AddCommand(string input) => _stringStringGroupsToTuple(_modCommandRegex.AddCommand, input);
    public string DelCommand(string input) => _firstGroup(_modCommandRegex.DelCommand, input);

    public Tuple<string, TimeSpan> AddMute(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.AddMute, input);
    public Tuple<string, TimeSpan> AddBan(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.AddBan, input);
    public Tuple<string, TimeSpan> AddMuteRegex(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.AddMuteRegex, input);
    public Tuple<string, TimeSpan> AddBanRegex(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.AddBanRegex, input);

    public string DelMute(string input) => _firstGroup(_modCommandRegex.DelMute, input);
    public string DelBan(string input) => _firstGroup(_modCommandRegex.DelBan, input);
    public string DelMuteRegex(string input) => _firstGroup(_modCommandRegex.DelMuteRegex, input);
    public string DelBanRegex(string input) => _firstGroup(_modCommandRegex.DelBanRegex, input);

    public Tuple<string, TimeSpan> Mute(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.Mute, input);
    public Tuple<string, TimeSpan> Ban(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.Ban, input);
    public Tuple<string, TimeSpan> Ipban(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.Ipban, input, true);
    public string Pardon(string input) => _firstGroup(_modCommandRegex.Pardon, input);

    public Tuple<string, TimeSpan> Nuke(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.Nuke, input);
    public Tuple<string, TimeSpan> RegexNuke(string input) => _numberUnitStringGroupsToTuple(_modCommandRegex.RegexNuke, input);

    private string _firstGroup(Regex regex, string input) => regex.Match(input).Groups[1].Value;

    private Tuple<string, string> _stringStringGroupsToTuple(Regex regex, string input) {
      var gc = regex.Match(input).Groups;
      var string1 = gc[1].Value;
      var string2 = gc[2].Value;
      return Tuple.Create(string1, string2);
    }

    private Tuple<string, TimeSpan> _numberUnitStringGroupsToTuple(Regex regex, string input, bool ip = false) {
      var gc = regex.Match(input).Groups;
      var number = gc[1].Value;
      var unit = gc[2].Value;
      var target = gc[3].Value;
      return Tuple.Create(target, _toTimespan(number, unit, ip));
    }

    private TimeSpan _toTimespan(string stringInt, string unit, bool ip) {
      int i;
      try {
        i = stringInt == "" ? 10 : int.Parse(stringInt);
      } catch (OverflowException) {
        i = int.MaxValue;
      }

      if (_modCommandRegex.Seconds.Any(x => x == unit)) {
        return TimeSpan.FromSeconds(i);
      }
      if (_modCommandRegex.Minutes.Any(x => x == unit)) {
        return TimeSpan.FromMinutes(i);
      }
      if (_modCommandRegex.Hours.Any(x => x == unit)) {
        return TimeSpan.FromHours(i);
      }
      if (_modCommandRegex.Days.Any(x => x == unit)) {
        return TimeSpan.FromDays(i);
      }
      if (_modCommandRegex.Perm.Any(x => x == unit)) {
        return TimeSpan.MaxValue;
      }
      if (unit == "") {
        if (ip && stringInt == "") return TimeSpan.MaxValue;
        return TimeSpan.FromMinutes(i);
      }
      throw new ArgumentException($"An invalid time passed the regex. StringInt:{stringInt}, unit:{unit}, ip:{ip}");
    }

  }
}
