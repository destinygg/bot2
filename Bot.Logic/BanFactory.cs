using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class BanFactory : BaseSendableFactory<Civilian, PublicMessage> {
    private readonly IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _repositoryBanFactory;
    private readonly IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _selfSpamBanFactory;

    public BanFactory(
      IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> repositoryBanFactory,
      IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> selfSpamBanFactory
    ) {
      _repositoryBanFactory = repositoryBanFactory;
      _selfSpamBanFactory = selfSpamBanFactory;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
      var outbox = new List<ISendable<ITransmittable>>();
      _repositoryBanFactory.Create(snapshot).Apply(s => outbox.AddRange(s));
      _selfSpamBanFactory.Create(snapshot).Apply(s => outbox.AddRange(s));
      return outbox;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(BanFactory)}.").Wrap().ToList();

  }
}
