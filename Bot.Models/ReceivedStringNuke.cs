using System;
using Bot.Logic.Contracts;
using Bot.Tools;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedStringNuke : ReceivedNuke {
    private readonly string _nukedString;
    public ReceivedStringNuke(ReceivedMessage message, IModCommandParser parser) : base(message) {
      Duration = parser.Nuke(message.Text).Item2;
      _nukedString = parser.Nuke(message.Text).Item1;
    }

    public override TimeSpan Duration { get; }

    protected override bool WillPunish(string possibleVictimText)
      => possibleVictimText.IgnoreCaseContains(_nukedString) || possibleVictimText.SimilarTo(_nukedString) >= Settings.NukeMinimumStringSimilarity;
  }
}
