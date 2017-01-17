using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Tools;
using Bot.Tools.Contracts;

namespace Bot.Logic {
  public class NukeLogic : INukeLogic {
    private readonly IModCommandRegex _modCommandRegex;
    private readonly IModCommandParser _modCommandParser;
    private readonly ITimeService _timeService;

    public NukeLogic(IModCommandRegex modCommandRegex, IModCommandParser modCommandParser, ITimeService timeService) {
      _modCommandRegex = modCommandRegex;
      _modCommandParser = modCommandParser;
      _timeService = timeService;
    }

    public IReadOnlyList<ISendable> Nuke(IReadOnlyList<IReceived> context, string phrase, TimeSpan duration)
      => _GetStringNukeVictims(context, phrase, _timeService.UtcNow).Select(u => new SendableMute(u, duration)).ToList();

    public IReadOnlyList<ISendable> RegexNuke(IReadOnlyList<IReceived> context, string phrase, TimeSpan duration)
      => _GetRegexNukeVictims(context, phrase, _timeService.UtcNow).Select(u => new SendableMute(u, duration)).ToList();

    private IEnumerable<IUser> _GetStringNukeVictims(IEnumerable<IReceived> context, string phrase, DateTime nukeTimestamp)
      => _GetNukeVictims(context, phrase, nukeTimestamp, s => s.SimilarTo(phrase) >= Settings.NukeMinimumStringSimilarity);

    private IEnumerable<IUser> _GetRegexNukeVictims(IEnumerable<IReceived> context, string regex, DateTime nukeTimestamp)
      => _GetNukeVictims(context, regex, nukeTimestamp, s => new Regex(regex, RegexOptions.IgnoreCase).IsMatch(s));

    private IEnumerable<IUser> _GetNukeVictims(IEnumerable<IReceived> context, string phrase, DateTime nukeTimestamp, Predicate<string> isMatchOrSimilar) => context
      .OfType<ReceivedMessage>()
      .Where(m => m.Timestamp.IsBeforeAndWithin(nukeTimestamp, Settings.NukeBlastRadius) && !m.FromMod())
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

      var stringVictims = stringsToAegis.SelectMany(n => _GetStringNukeVictims(context, n, firstNukeTimeStamp));
      var regexVictims = regexesToAegis.SelectMany(r => _GetRegexNukeVictims(context, r, firstNukeTimeStamp));

      //TODO consider checking if these are actual victims?
      var allVictims = stringVictims.Concat(regexVictims).Distinct();
      var alreadyUnMuteBanned = context.OfType<ReceivedUnMuteBan>().Select(umb => umb.Target);
      return allVictims.Except(alreadyUnMuteBanned).Select(v => new SendableUnMuteBan(v)).ToList();
    }

    private bool IsBeforeWithAegisWindow(IReceived received, DateTime dateTime)
      => received.Timestamp >= dateTime.Subtract(Settings.AegisRadiusAroundFirstNuke);
  }
}
