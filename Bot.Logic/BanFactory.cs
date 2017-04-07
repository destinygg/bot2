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
      if (message.Transmission.Text.Contains("banplox")) {
        outbox.Add(new SendablePublicMessage($"{message.Sender.Nick} banned for saying {message.Transmission.Text}"));
      }

      foreach (var autoPunishment in _repository.Query(db => db.AutoPunishments.GetAllMutedString())) {
        if (message.Transmission.Text.SimilarTo(autoPunishment.Term) >= _settings.MinimumPunishmentSimilarity) {
          var punishedUser = autoPunishment.PunishedUsers.SingleOrDefault(x => x.Nick == message.Sender.Nick);

          var duration = autoPunishment.Duration;
          if (punishedUser != null)
            duration = autoPunishment.Duration.Multiply(Math.Pow(2, punishedUser.Count));

          _repository.Command(r => r.PunishedUsers.Increment(message.Sender.Nick, autoPunishment.Term));
          outbox.Add(new SendableMute(message.Sender, duration));
        }
      }
      return outbox;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(BanFactory)}.").Wrap().ToList();

    //IReadOnlyCollection?
    //public IReadOnlyList<ISendable> Create(ISnapshot snapshot) =>
    //  Herpderp(snapshot).ToList();

    //private IEnumerable<ISendable> Herpderp(ISnapshot snapshot) {
    //  var message = snapshot.First as ReceivedMessage;
    //  if (message != null && message.Text.Contains("banplox")) {
    //    yield return new SendablePublicMessage($"{message.Sender.Nick} banned for saying {message.Text}");
    //  }
    //}

  }
}
