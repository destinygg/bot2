using Bot.Models;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic.ReceivedVisitor {
  public class ModeratorReceivedVisitor : BaseReceivedVisitor<Moderator> {

    public ModeratorReceivedVisitor() { }

    public override DelegatedSnapshotFactory Visit<TVisitedUser, TTransmission>(Received<TVisitedUser, TTransmission> received) {
      throw new System.NotImplementedException();
    }

  }
}
