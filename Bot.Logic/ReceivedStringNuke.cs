using System;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class ReceivedStringNuke : ReceivedNuke {
    private readonly string _nukedString;
    public ReceivedStringNuke(IReceivedMessage<Moderator> message, ITimeService timeService, IModCommandParser parser) : base(message, timeService) {
      Duration = parser.Nuke(message.Text).Item2;
      _nukedString = parser.Nuke(message.Text).Item1;
    }

    public override TimeSpan Duration { get; }

    protected override bool MatchesNukedTerm(string possibleVictimText) => 
      possibleVictimText.IgnoreCaseContains(_nukedString) || 
      possibleVictimText.RemoveWhitespace().IgnoreCaseContains(_nukedString) ||
      possibleVictimText.SimilarTo(_nukedString) >= Settings.NukeMinimumStringSimilarity;

  }
}
