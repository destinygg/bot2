using System;
using System.Text.RegularExpressions;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class NukeFactory : IFactory<IReceived<Moderator, IMessage>, Nuke> {
    private readonly IModCommandRegex _modCommandRegex;
    private readonly IModCommandParser _parser;
    private readonly ISettings _settings;

    public NukeFactory(IModCommandRegex modCommandRegex, IModCommandParser parser, ISettings settings) {
      _modCommandRegex = modCommandRegex;
      _parser = parser;
      _settings = settings;
    }

    public Nuke Create(IReceived<Moderator, IMessage> message) {
      if (message.IsMatch(_modCommandRegex.Nuke)) {
        var phraseDuration = _parser.Nuke(message.Transmission.Text);
        var matchesNukedTerm = _StringNuke(phraseDuration.Item1);
        var duration = phraseDuration.Item2;
        return new Nuke(message.Timestamp, duration, matchesNukedTerm);
      } else if (message.IsMatch(_modCommandRegex.RegexNuke)) {
        var phraseDuration = _parser.RegexNuke(message.Transmission.Text);
        var regex = new Regex(phraseDuration.Item1, RegexOptions.IgnoreCase);
        var matchesNukedTerm = _RegexNuke(regex);
        var duration = phraseDuration.Item2;
        return new Nuke(message.Timestamp, duration, matchesNukedTerm);
      } else throw new ArgumentException($"Unable to parse this Nuke: {message.Transmission.Text}");
    }

    private Predicate<string> _RegexNuke(Regex nukedRegex) => nukedRegex.IsMatch;

    private Predicate<string> _StringNuke(string nukedString) => possibleVictimText =>
      possibleVictimText.IgnoreCaseContains(nukedString) ||
      possibleVictimText.RemoveWhitespace().IgnoreCaseContains(nukedString) ||
      possibleVictimText.SimilarTo(nukedString) >= _settings.NukeMinimumStringSimilarity;
  }
}
