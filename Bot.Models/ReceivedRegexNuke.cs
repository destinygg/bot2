using System;
using System.Text.RegularExpressions;
using Bot.Logic.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedRegexNuke : ReceivedNuke {
    private readonly Regex _nukedRegex;
    public ReceivedRegexNuke(ReceivedMessage message, ITimeService timeService, IModCommandParser parser) : base(message, timeService) {
      _nukedRegex = new Regex(parser.RegexNuke(message.Text).Item1, RegexOptions.IgnoreCase);
      Duration = parser.RegexNuke(message.Text).Item2;
    }

    public override TimeSpan Duration { get; }

    protected override bool WillPunish(string possibleVictimText)
      => _nukedRegex.IsMatch(possibleVictimText);
  }
}
