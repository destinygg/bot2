using System;
using System.Text.RegularExpressions;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class ReceivedRegexNuke : ReceivedNuke {
    private readonly Regex _nukedRegex;
    public ReceivedRegexNuke(IReceivedMessage<Moderator> message, ITimeService timeService, IModCommandParser parser) : base(message, timeService) {
      _nukedRegex = new Regex(parser.RegexNuke(message.Text).Item1, RegexOptions.IgnoreCase);
      Duration = parser.RegexNuke(message.Text).Item2;
    }

    public override TimeSpan Duration { get; }

    protected override bool MatchesNukedTerm(string possibleVictimText) => _nukedRegex.IsMatch(possibleVictimText);

  }
}
