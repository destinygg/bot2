using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.ReceivedVisitor {
  public class UserVisitor : IUserVisitor<IReceivedVisitor<SnapshotFactory>> {
    private readonly ModeratorReceivedVisitor _moderatorReceivedVisitor;
    private readonly CivilianReceivedVisitor _civilianReceivedVisitor;

    public UserVisitor(ModeratorReceivedVisitor moderatorReceivedVisitor, CivilianReceivedVisitor civilianReceivedVisitor) {
      _moderatorReceivedVisitor = moderatorReceivedVisitor;
      _civilianReceivedVisitor = civilianReceivedVisitor;
    }

    public IReceivedVisitor<SnapshotFactory> Visit(Moderator moderator) => _moderatorReceivedVisitor;

    public IReceivedVisitor<SnapshotFactory> Visit(Civilian civilian) => _civilianReceivedVisitor;
  }
}
