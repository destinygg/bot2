using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic.SnapshotVisitor {
  public class SnapshotVisitor : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _modCommandFactory;
    private readonly IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> _commandFactory;
    private readonly IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> _civilianPublicMessageToSendablesFactory;
    private readonly IFactory<ISendable<ITransmittable>, Moderator, ISendable<ITransmittable>> _publicToPrivateMessageFactory;
    private readonly ILogger _logger;

    public SnapshotVisitor(
      IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> modCommandFactory,
      IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>> commandFactory,
      IFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> civilianPublicMessageToSendablesFactory,
      IFactory<ISendable<ITransmittable>, Moderator, ISendable<ITransmittable>> publicToPrivateMessageFactory,
      ILogger logger
    ) {
      _modCommandFactory = modCommandFactory;
      _commandFactory = commandFactory;
      _civilianPublicMessageToSendablesFactory = civilianPublicMessageToSendablesFactory;
      _publicToPrivateMessageFactory = publicToPrivateMessageFactory;
      _logger = logger;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Civilian, PublicMessage> snapshot) =>
      _civilianPublicMessageToSendablesFactory.Create(snapshot);

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, PublicMessage> snapshot) =>
      _modCommandFactory.Create(snapshot).Concat(_commandFactory.Create(snapshot)).ToList();

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, PrivateMessage> snapshot) =>
      _modCommandFactory.Create(snapshot).Concat(_commandFactory.Create(snapshot)).Select(s => _publicToPrivateMessageFactory.Create(s, snapshot.Latest.Sender)).ToList();

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, ErrorMessage> snapshot) {
      _logger.LogError(snapshot.Latest.Transmission.Text);
      return new SendablePublicMessage(snapshot.Latest.Transmission.Text).Wrap().ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, Pardon> snapshot) {
      _logger.LogError(snapshot.Latest.Transmission.ToString());
      return new List<ISendable<ITransmittable>>();
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<Moderator, InitialUsers> initialUsers) => new List<ISendable<ITransmittable>>();

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<IUser, Join> join) => new List<ISendable<ITransmittable>>();

    public IReadOnlyList<ISendable<ITransmittable>> Visit(ISnapshot<IUser, Quit> join) => new List<ISendable<ITransmittable>>();

  }
}
