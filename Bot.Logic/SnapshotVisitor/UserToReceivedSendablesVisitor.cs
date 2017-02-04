using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SnapshotVisitor {
  public class UserVisitor : IUserVisitor<ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>> {
    private readonly ModeratorSnapshotVisitor _moderatorSnapshotVisitor;
    private readonly CivilianSnapshotVisitor _civilianSnapshotVisitor;

    public UserVisitor(ModeratorSnapshotVisitor moderatorSnapshotVisitor, CivilianSnapshotVisitor civilianSnapshotVisitor) {
      _moderatorSnapshotVisitor = moderatorSnapshotVisitor;
      _civilianSnapshotVisitor = civilianSnapshotVisitor;
    }

    public ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> Visit(Moderator moderator) => _moderatorSnapshotVisitor;

    public ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>> Visit(Civilian civilian) => _civilianSnapshotVisitor;
  }
}
