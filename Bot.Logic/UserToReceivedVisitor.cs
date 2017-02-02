using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class UserToReceivedVisitor : IUserVisitor<IReceivedVisitor<Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>>> {
    private readonly ModeratorReceivedToSendablesVisitor _moderatorReceivedToSendablesVisitor;
    private readonly CivilianReceivedToSendablesVisitor _civilianReceivedToSendablesVisitor;

    public UserToReceivedVisitor(ModeratorReceivedToSendablesVisitor moderatorReceivedToSendablesVisitor, CivilianReceivedToSendablesVisitor civilianReceivedToSendablesVisitor) {
      _moderatorReceivedToSendablesVisitor = moderatorReceivedToSendablesVisitor;
      _civilianReceivedToSendablesVisitor = civilianReceivedToSendablesVisitor;
    }

    public IReceivedVisitor<Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>> Visit(Moderator moderator) => _moderatorReceivedToSendablesVisitor;

    public IReceivedVisitor<Func<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>> Visit(Civilian civilian) => _civilianReceivedToSendablesVisitor;
  }
}
