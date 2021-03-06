﻿using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class PunishmentFactory : BaseSendableFactory<Civilian, PublicMessage> {
    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _singleLineSpamPunishmentFactory;
    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _repositoryPunishmentFactory;
    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _selfSpamPunishmentFactory;
    private readonly IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _longSpamPunishmentFactory;

    public PunishmentFactory(
      IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> singleLineSpamPunishmentFactory,
      IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> repositoryPunishmentFactory,
      IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> selfSpamPunishmentFactory,
      IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> longSpamPunishmentFactory
    ) {
      _singleLineSpamPunishmentFactory = singleLineSpamPunishmentFactory;
      _repositoryPunishmentFactory = repositoryPunishmentFactory;
      _selfSpamPunishmentFactory = selfSpamPunishmentFactory;
      _longSpamPunishmentFactory = longSpamPunishmentFactory;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
      var outbox = new List<ISendable<ITransmittable>>();
      _singleLineSpamPunishmentFactory.Create(snapshot).Apply(s => outbox.AddRange(s));
      _repositoryPunishmentFactory.Create(snapshot).Apply(s => outbox.AddRange(s));
      _selfSpamPunishmentFactory.Create(snapshot).Apply(s => outbox.AddRange(s));
      _longSpamPunishmentFactory.Create(snapshot).Apply(s => outbox.AddRange(s));
      return _processMaximumPunishment(outbox);
    }

    private List<ISendable<ITransmittable>> _processMaximumPunishment(List<ISendable<ITransmittable>> punishments) {
      var maxPunishment = punishments.Cast<ISendable<Punishment>>().ArgMax(x => x.Transmission.Duration);
      var responses = new List<ISendable<ITransmittable>>();
      if (maxPunishment == null) return responses;
      responses.Add(maxPunishment);
      if (!string.IsNullOrWhiteSpace(maxPunishment.Transmission.Reason)) {
        responses.Add(new SendablePublicMessage(maxPunishment.Transmission.Reason));
      }
      return responses;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(PunishmentFactory)}.").Wrap().ToList();

  }
}
