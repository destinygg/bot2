using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;
using Bot.Tools;

namespace Bot.Logic {
  public class ModCommandLogic : IModCommandLogic {
    private readonly ILogger _logger;
    private readonly IModCommandRegex _modCommandRegex;
    private readonly IModCommandParser _modCommandParser;

    public ModCommandLogic(ILogger logger, IModCommandRegex modCommandRegex, IModCommandParser modCommandParser) {
      _logger = logger;
      _modCommandRegex = modCommandRegex;
      _modCommandParser = modCommandParser;
    }

    public ISendable Long(IReadOnlyList<IReceived> context) {
      _logger.LogInformation($"Long running process beginning, context length: {context.Count()}");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("1");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("2");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("3");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation($"Long running process ending, context length: {context.Count()}");
      return new SendableMessage("This is a debug command; output appears in log.");
    }

    public ISendable Sing() => new SendableMessage("/me sings a song");

    public IReadOnlyList<ISendable> Nuke(IReadOnlyList<IReceived> context, string phrase, TimeSpan duration)
      => _GetStringNukeVictims(context, phrase).Select(u => new SendableMute(u, duration)).ToList();

    private IEnumerable<IUser> _GetStringNukeVictims(IEnumerable<IReceived> context, string phrase)
      => _GetNukeVictims(context, phrase, s => s.SimilarTo(phrase) >= Settings.NukeMinimumStringSimilarity);

    private IEnumerable<IUser> _GetRegexNukeVictims(IEnumerable<IReceived> context, string regex)
      => _GetNukeVictims(context, regex, s => new Regex(regex, RegexOptions.IgnoreCase).IsMatch(s));

    private IEnumerable<IUser> _GetNukeVictims(IEnumerable<IReceived> context, string phrase, Predicate<string> isMatchOrSimilar) => context
      .OfType<ReceivedMessage>()
      .Where(m => m.Timestamp.IsWithin(Settings.NukeBlastRadius) && !m.FromMod())
      .Where(m => m.Text.Contains(phrase, StringComparison.InvariantCultureIgnoreCase) || isMatchOrSimilar(m.Text))
      .Select(m => m.Sender).Distinct();

    public IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived> context) {
      var modMessages = context.OfType<ReceivedMessage>().Where(m => m.FromMod()).ToList();
      var nukedStringsMessages = modMessages.Where(rm => rm.IsMatch(_modCommandRegex.Nuke)).ToList();
      var nukedRegexesMessages = modMessages.Where(rm => rm.IsMatch(_modCommandRegex.RegexNuke)).ToList();

      var allNukedModMessages = nukedStringsMessages.Concat(nukedRegexesMessages).OrderBy(m => m.Timestamp);
      var firstNuke = allNukedModMessages.FirstOrDefault();
      if (firstNuke == null) return new List<ISendable>();
      var firstNukeTimeStamp = firstNuke.Timestamp;

      var stringsToAegis = nukedStringsMessages.Where(n => IsBeforeWithAegisWindow(n, firstNukeTimeStamp)).Select(rm => _modCommandParser.Nuke(rm.Text).Item1);
      var regexesToAegis = nukedRegexesMessages.Where(n => IsBeforeWithAegisWindow(n, firstNukeTimeStamp)).Select(rm => _modCommandParser.RegexNuke(rm.Text).Item1);

      context = context.Where(r => IsBeforeWithAegisWindow(r, firstNukeTimeStamp)).ToList();

      var stringVictims = stringsToAegis.SelectMany(n => _GetStringNukeVictims(context, n));
      var regexVictims = regexesToAegis.SelectMany(r => _GetRegexNukeVictims(context, r));

      //TODO consider checking if these are actual victims?
      var allVictims = stringVictims.Concat(regexVictims).Distinct();
      var alreadyUnMuteBanned = context.OfType<ReceivedUnMuteBan>().Select(umb => umb.Target);
      return allVictims.Except(alreadyUnMuteBanned).Select(v => new SendableUnMuteBan(v)).ToList();
    }

    private bool IsBeforeWithAegisWindow(IReceived received, DateTime dateTime)
      => received.Timestamp >= dateTime.Subtract(Settings.AegisRadiusAroundFirstNuke);
  }
}
