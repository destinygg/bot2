using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class UserToReceivedVisitor : IUserVisitor<IReceivedVisitor<SendablesFactory>> {
    private readonly ModeratorReceivedToSendablesVisitor _moderatorReceivedToSendablesVisitor;
    private readonly CivilianReceivedToSendablesVisitor _civilianReceivedToSendablesVisitor;

    public UserToReceivedVisitor(ModeratorReceivedToSendablesVisitor moderatorReceivedToSendablesVisitor, CivilianReceivedToSendablesVisitor civilianReceivedToSendablesVisitor) {
      _moderatorReceivedToSendablesVisitor = moderatorReceivedToSendablesVisitor;
      _civilianReceivedToSendablesVisitor = civilianReceivedToSendablesVisitor;
    }

    public IReceivedVisitor<SendablesFactory> Visit(Moderator moderator) => _moderatorReceivedToSendablesVisitor;

    public IReceivedVisitor<SendablesFactory> Visit(Civilian civilian) => _civilianReceivedToSendablesVisitor;
  }
}
