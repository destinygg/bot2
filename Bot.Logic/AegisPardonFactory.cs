using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class AegisPardonFactory : NukeAegisSendableFactoryBase, IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IModCommandRegex _modCommandRegex;
    private readonly IFactory<IReceived<Moderator, IMessage>, Nuke> _nukeFactory;

    public AegisPardonFactory(IModCommandRegex modCommandRegex, IFactory<IReceived<Moderator, IMessage>, Nuke> nukeFactory, ISettings settings, ITimeService timeService) : base(settings, timeService) {
      _modCommandRegex = modCommandRegex;
      _nukeFactory = nukeFactory;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      var modMessages = context.OfType<IReceived<Moderator, IMessage>>().ToList();
      var nukes = _GetStringNukes(modMessages).Concat(_GetRegexNukes(modMessages));
      var victims = nukes.SelectMany(n => GetCurrentVictims(n, context));

      //TODO consider checking if these are actual victims?
      var alreadyPardoned = context.OfType<ReceivedPardon>().Select(umb => umb.Target);
      return victims.Except(alreadyPardoned).Select(v => new SendablePardon(v)).ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(AegisPardonFactory)}.").Wrap().ToList();

    private IEnumerable<Nuke> _GetStringNukes(IEnumerable<IReceived<Moderator, IMessage>> modMessages) => modMessages
      .Where(m => m.IsMatch(_modCommandRegex.Nuke))
      .Select(rm => _nukeFactory.Create(rm));

    private IEnumerable<Nuke> _GetRegexNukes(IEnumerable<IReceived<Moderator, IMessage>> modMessages) => modMessages
      .Where(m => m.IsMatch(_modCommandRegex.RegexNuke))
      .Select(rm => _nukeFactory.Create(rm));
  }
}
