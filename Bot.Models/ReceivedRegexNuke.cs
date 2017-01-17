using System;
using System.Text.RegularExpressions;
using Bot.Logic.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedRegexNuke : ReceivedNuke {
    private readonly Regex _nukedRegex;
    public ReceivedRegexNuke(ReceivedMessage message, IModCommandParser parser, ITimeService timeService) : base(message, timeService) {
      _nukedRegex = new Regex(parser.Nuke(message.Text).Item1, RegexOptions.IgnoreCase);
      Duration = parser.Nuke(message.Text).Item2;
    }

    public override TimeSpan Duration { get; }

    public override bool IsMatch(string victimText)
      => _nukedRegex.IsMatch(victimText);
  }
}
