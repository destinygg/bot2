using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bot.Database.Entities;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class RepositoryPunishmentFactory : IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly ILogger _logger;

    public RepositoryPunishmentFactory(IQueryCommandService<IUnitOfWork> unitOfWork, ILogger logger) {
      _unitOfWork = unitOfWork;
      _logger = logger;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
      var outbox = new List<ISendable<ITransmittable>>();
      var message = snapshot.Latest;

      _unitOfWork.Query(r => r.InMemory.Nukes)
        .Where(nuke => nuke.MatchesNukedTerm(message.Transmission.Text))
        .Select(nuke => new SendableMute(message.Sender, nuke.Duration))
        .Apply(outbox.AddRange);
      var autoPunishments = _unitOfWork.Query(r => r.AutoPunishments.GetAllWithUser);
      Func<AutoPunishment, bool> stringFilter = autoPunishment => message.Transmission.Text.Contains(autoPunishment.Term);
      Func<AutoPunishment, bool> regexFilter = autoPunishment => message.IsMatch(new Regex(autoPunishment.Term, RegexOptions.IgnoreCase));
      ConstructPunishment(message, autoPunishments.Where(x => x.Type == AutoPunishmentType.MutedString), stringFilter, (x, y, z) => new SendableMute(x, y, z)).Apply(outbox.AddRange);
      ConstructPunishment(message, autoPunishments.Where(x => x.Type == AutoPunishmentType.MutedRegex), regexFilter, (x, y, z) => new SendableMute(x, y, z)).Apply(outbox.AddRange);
      ConstructPunishment(message, autoPunishments.Where(x => x.Type == AutoPunishmentType.BannedString), stringFilter, (x, y, z) => new SendableBan(x, y, z)).Apply(outbox.AddRange);
      ConstructPunishment(message, autoPunishments.Where(x => x.Type == AutoPunishmentType.BannedRegex), regexFilter, (x, y, z) => new SendableBan(x, y, z)).Apply(outbox.AddRange);
      return outbox;
    }

    private IEnumerable<ISendable<Punishment>> ConstructPunishment(
      IReceived<Civilian, PublicMessage> message,
      IEnumerable<AutoPunishment> autoPunishments,
      Func<AutoPunishment, bool> filter,
      Func<Civilian, TimeSpan, string, ISendable<Punishment>> punishmentCtor
    ) => autoPunishments
      .Where(filter)
      .Select(autoPunishment => CalculatePunishment(message.Sender, autoPunishment, punishmentCtor));

    private ISendable<Punishment> CalculatePunishment(Civilian sender, AutoPunishment autoPunishment, Func<Civilian, TimeSpan, string, ISendable<Punishment>> punishmentCtor) {
      var punishedUser = autoPunishment.PunishedUsers.SingleOrDefault(u => u.Nick == sender.Nick);
      var duration = autoPunishment.Duration;
      var reason = $"{duration.ToPretty(_logger)} for prohibited phrase";
      if (punishedUser != null) {
        duration = autoPunishment.Duration.Multiply(Math.Pow(2, punishedUser.Count));
        reason = punishedUser.Count == 1
          ? $"{duration.ToPretty(_logger)} for prohibited phrase; your time has doubled. Future sanctions will not be explicitly justified."
          : "";
      }
      _unitOfWork.Command(r => r.PunishedUsers.Increment(sender.Nick, autoPunishment.Term));
      return punishmentCtor(sender, duration, reason);
    }

    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new List<ISendable<ITransmittable>>();
  }
}
