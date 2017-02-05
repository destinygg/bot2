using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.ReceivedVisitor {
  public class UserVisitor : IUserVisitor<IReceivedVisitor<DelegatedSnapshotFactory>> {
    private readonly ModeratorReceivedVisitor _moderatorReceivedVisitor;
    private readonly CivilianReceivedVisitor _civilianReceivedVisitor;

    public UserVisitor(ModeratorReceivedVisitor moderatorReceivedVisitor, CivilianReceivedVisitor civilianReceivedVisitor) {
      _moderatorReceivedVisitor = moderatorReceivedVisitor;
      _civilianReceivedVisitor = civilianReceivedVisitor;
    }

    public IReceivedVisitor<DelegatedSnapshotFactory> Visit(Moderator moderator) => _moderatorReceivedVisitor;

    public IReceivedVisitor<DelegatedSnapshotFactory> Visit(Civilian civilian) => _civilianReceivedVisitor;
  }
}
