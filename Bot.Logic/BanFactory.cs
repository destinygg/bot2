using System;
using System.Collections.Generic;
using System.Linq;
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

      outbox.AddRange(ConstructPunishment(message, r => r.AutoPunishments.GetAllMutedString(), (x, y) => new SendableMute(x, y)));
      outbox.AddRange(ConstructPunishment(message, r => r.AutoPunishments.GetAllMutedRegex(), (x, y) => new SendableMute(x, y)));
      outbox.AddRange(ConstructPunishment(message, r => r.AutoPunishments.GetAllBannedString(), (x, y) => new SendableBan(x, y)));
      outbox.AddRange(ConstructPunishment(message, r => r.AutoPunishments.GetAllBannedRegex(), (x, y) => new SendableBan(x, y)));

      return outbox;
    }

    private IEnumerable<ISendable<Punishment>> ConstructPunishment(
        IReceived<Civilian, PublicMessage> message,
        Func<IUnitOfWork, IEnumerable<AutoPunishment>> query,
        Func<Civilian, TimeSpan, ISendable<Punishment>> punishmentCtor
      ) => _repository
      .Query(query)
      .Where(autoPunishment => message.Transmission.Text.SimilarTo(autoPunishment.Term) >= _settings.MinimumPunishmentSimilarity)
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
