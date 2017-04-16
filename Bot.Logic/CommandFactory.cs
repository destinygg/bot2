using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class CommandFactory : BaseSendableFactory<IUser, IMessage> {
    private readonly IQueryCommandService<IUnitOfWork> _repository;
    private readonly ITimeService _timeService;

    public CommandFactory(IQueryCommandService<IUnitOfWork> repository, ITimeService timeService) {
      _repository = repository;
      _timeService = timeService;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<IUser, IMessage> snapshot) {
      var outbox = new List<ISendable<PublicMessage>>();
      var message = snapshot.Latest;

      foreach (var customCommand in _repository.Query(db => db.CustomCommand.GetAll)) {
        if (message.StartsWith($"!{customCommand.Command}"))
          return new SendablePublicMessage(customCommand.Response).Wrap().ToList();
      }

      if (message.StartsWith("!time")) {
        outbox.Add(new SendablePublicMessage($"{_timeService.DestinyNow.ToShortTimeString()} Central Steven Time"));
      }
      return outbox;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(CommandFactory)}.").Wrap().ToList();
  }
}
