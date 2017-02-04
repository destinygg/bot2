using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class UserToReceivedSendablesVisitor : IUserVisitor<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>> {
    private readonly ModeratorReceivedToSendablesVisitor _moderatorReceivedToSendablesVisitor;
    private readonly CivilianReceivedToSendablesVisitor _civilianReceivedToSendablesVisitor;

    public UserToReceivedSendablesVisitor(ModeratorReceivedToSendablesVisitor moderatorReceivedToSendablesVisitor, CivilianReceivedToSendablesVisitor civilianReceivedToSendablesVisitor) {
      _moderatorReceivedToSendablesVisitor = moderatorReceivedToSendablesVisitor;
      _civilianReceivedToSendablesVisitor = civilianReceivedToSendablesVisitor;
    }

    public ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> Visit(Moderator moderator) => _moderatorReceivedToSendablesVisitor;

    public ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> Visit(Civilian civilian) => _civilianReceivedToSendablesVisitor;
  }
}
