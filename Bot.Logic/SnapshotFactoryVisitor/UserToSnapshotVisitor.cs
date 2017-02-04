using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SnapshotFactoryVisitor {
  public class UserToSnapshotVisitor : IUserVisitor<IReceivedVisitor<SnapshotFactory>> {
    private readonly ReceivedFromModeratorToSnapshotVisitor _receivedFromModeratorToSnapshotVisitor;
    private readonly ReceivedFromCivilianToSnapshotVisitor _receivedFromCivilianToSnapshotVisitor;

    public UserToSnapshotVisitor(ReceivedFromModeratorToSnapshotVisitor receivedFromModeratorToSnapshotVisitor, ReceivedFromCivilianToSnapshotVisitor receivedFromCivilianToSnapshotVisitor) {
      _receivedFromModeratorToSnapshotVisitor = receivedFromModeratorToSnapshotVisitor;
      _receivedFromCivilianToSnapshotVisitor = receivedFromCivilianToSnapshotVisitor;
    }

    public IReceivedVisitor<SnapshotFactory> Visit(Moderator moderator) => _receivedFromModeratorToSnapshotVisitor;

    public IReceivedVisitor<SnapshotFactory> Visit(Civilian civilian) => _receivedFromCivilianToSnapshotVisitor;
  }
}
