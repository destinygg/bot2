using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SendablesFactoryVisitor {
  public class UserToReceivedSendablesVisitor : IUserVisitor<IReceivedVisitor<SendablesFactory>> {
    private readonly ModeratorReceivedToSendablesVisitor _moderatorReceivedToSendablesVisitor;
    private readonly CivilianReceivedToSendablesVisitor _civilianReceivedToSendablesVisitor;

    public UserToReceivedSendablesVisitor(ModeratorReceivedToSendablesVisitor moderatorReceivedToSendablesVisitor, CivilianReceivedToSendablesVisitor civilianReceivedToSendablesVisitor) {
      _moderatorReceivedToSendablesVisitor = moderatorReceivedToSendablesVisitor;
      _civilianReceivedToSendablesVisitor = civilianReceivedToSendablesVisitor;
    }

    public IReceivedVisitor<SendablesFactory> Visit(Moderator moderator) => _moderatorReceivedToSendablesVisitor;

    public IReceivedVisitor<SendablesFactory> Visit(Civilian civilian) => _civilianReceivedToSendablesVisitor;
  }
}
