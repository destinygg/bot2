using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class UserToReceivedVisitor : IUserVisitor<IReceivedVisitor> {
    private readonly ModeratorReceivedVisitor _moderatorReceivedVisitor;
    private readonly CivilianReceivedVisitor _civilianReceivedVisitor;

    public UserToReceivedVisitor(ModeratorReceivedVisitor moderatorReceivedVisitor, CivilianReceivedVisitor civilianReceivedVisitor) {
      _moderatorReceivedVisitor = moderatorReceivedVisitor;
      _civilianReceivedVisitor = civilianReceivedVisitor;
    }

    public IReceivedVisitor Visit(Moderator moderator) => _moderatorReceivedVisitor;

    public IReceivedVisitor Visit(Civilian civilian) => _civilianReceivedVisitor;
  }
}
