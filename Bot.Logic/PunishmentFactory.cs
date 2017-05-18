using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class PunishmentFactory : BaseSendableFactory<Civilian, PublicMessage> {
    private readonly IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _repositoryPunishmentFactory;
    private readonly IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _selfSpamPunishmentFactory;

    public PunishmentFactory(
      IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> repositoryPunishmentFactory,
      IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> selfSpamPunishmentFactory
    ) {
      _repositoryPunishmentFactory = repositoryPunishmentFactory;
      _selfSpamPunishmentFactory = selfSpamPunishmentFactory;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
      var outbox = new List<ISendable<ITransmittable>>();
      _repositoryPunishmentFactory.Create(snapshot).Apply(s => outbox.AddRange(s));
      _selfSpamPunishmentFactory.Create(snapshot).Apply(s => outbox.AddRange(s));
      return outbox;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(PunishmentFactory)}.").Wrap().ToList();

  }
}
