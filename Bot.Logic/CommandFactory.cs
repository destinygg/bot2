using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class CommandFactory : BaseSendableFactory<IUser, IMessage> {
    private readonly IQueryCommandService<IUnitOfWork> _repository;
    private readonly ICommandLogic _commandLogic;

    public CommandFactory(IQueryCommandService<IUnitOfWork> repository, ICommandLogic commandLogic) {
      _repository = repository;
      _commandLogic = commandLogic;
    }

    public override IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<IUser, IMessage> snapshot) {
      var message = snapshot.Latest;

      foreach (var customCommand in _repository.Query(db => db.CustomCommand.GetAll)) {
        if (message.StartsWith($"!{customCommand.Command}", $"! {customCommand.Command}"))
          return new SendablePublicMessage(customCommand.Response).Wrap().ToList();
      }

      if (message.StartsWith("!time"))
        return _commandLogic.Time().Wrap().ToList();
      if (message.StartsWith("!sch", "!calen"))
        return _commandLogic.Schedule().Wrap().ToList();
      if (message.StartsWith("!blog", "!blag"))
        return _commandLogic.Blog().Wrap().ToList();
      if (message.StartsWith("!strim", "!stream"))
        return _commandLogic.Streams().ToList();
      if (message.StartsWith("!twit", "!tweet", "!twat"))
        return _commandLogic.TwitterDestiny().ToList();
      if (message.StartsWith("!aslan", "! aslan"))
        return _commandLogic.TwitterAslan().ToList();
      if (message.StartsWith("!song"))
        return _commandLogic.Song().ToList();
      if (message.StartsWith("!pastsong", "!lastsong", "!previoussong", "!earliersong"))
        return _commandLogic.PreviousSong().ToList();
      if (message.StartsWith("!live"))
        return _commandLogic.Live().ToList();

      return new List<ISendable<PublicMessage>>();
    }

    public override IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(CommandFactory)}.").Wrap().ToList();
  }
}
