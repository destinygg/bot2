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

namespace Bot.Logic {
  public class BanFactory : BaseSendableFactory<Civilian, PublicMessage> {
    private readonly IQueryCommandService<IUnitOfWork> _repository;
    private readonly ISettings _settings;

    public BanFactory(IQueryCommandService<IUnitOfWork> repository, ISettings settings) {
      _repository = repository;
      _settings = settings;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
      var outbox = new List<ISendable<ITransmittable>>();
      var message = snapshot.Latest;

      _repository.Query(r => r.InMemory.Nukes)
        .Where(nuke => nuke.MatchesNukedTerm(message.Transmission.Text))
        .Select(nuke => new SendableMute(message.Sender, nuke.Duration))
        .Apply(outbox.AddRange);
      var autoPunishments = _repository.Query(r => r.AutoPunishments.GetAllWithUser);
      Func<AutoPunishment, bool> stringFilter = autoPunishment => message.Transmission.Text.SimilarTo(autoPunishment.Term) >= _settings.MinimumPunishmentSimilarity;
      Func<AutoPunishment, bool> regexFilter = autoPunishment => message.IsMatch(new Regex(autoPunishment.Term, RegexOptions.IgnoreCase));
      ConstructPunishment(message, autoPunishments.Where(x => x.Type == AutoPunishmentType.MutedString), stringFilter, (x, y) => new SendableMute(x, y)).Apply(outbox.AddRange);
      ConstructPunishment(message, autoPunishments.Where(x => x.Type == AutoPunishmentType.MutedRegex), regexFilter, (x, y) => new SendableMute(x, y)).Apply(outbox.AddRange);
      ConstructPunishment(message, autoPunishments.Where(x => x.Type == AutoPunishmentType.BannedString), stringFilter, (x, y) => new SendableBan(x, y)).Apply(outbox.AddRange);
      ConstructPunishment(message, autoPunishments.Where(x => x.Type == AutoPunishmentType.BannedRegex), regexFilter, (x, y) => new SendableBan(x, y)).Apply(outbox.AddRange);

      return outbox;
    }

    private IEnumerable<ISendable<Punishment>> ConstructPunishment(
        IReceived<Civilian, PublicMessage> message,
        IEnumerable<AutoPunishment> autoPunishments,
        Func<AutoPunishment, bool> filter,
        Func<Civilian, TimeSpan, ISendable<Punishment>> punishmentCtor
      ) => autoPunishments
      .Where(filter)
      .Select(autoPunishment => CalculatePunishment(message.Sender, autoPunishment, punishmentCtor));

    private ISendable<Punishment> CalculatePunishment(Civilian sender, AutoPunishment autoPunishment, Func<Civilian, TimeSpan, ISendable<Punishment>> punishmentCtor) {
      var punishedUser = autoPunishment.PunishedUsers.SingleOrDefault(u => u.Nick == sender.Nick);
      var duration = autoPunishment.Duration;
      if (punishedUser != null)
        duration = autoPunishment.Duration.Multiply(Math.Pow(2, punishedUser.Count));
      _repository.Command(r => r.PunishedUsers.Increment(sender.Nick, autoPunishment.Term));
      return punishmentCtor(sender, duration);
    }

    public override IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(BanFactory)}.").Wrap().ToList();

  }
}
