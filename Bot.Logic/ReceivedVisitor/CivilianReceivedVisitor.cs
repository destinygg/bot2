using Bot.Models;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic.ReceivedVisitor {
  public class CivilianReceivedVisitor : BaseReceivedVisitor<Civilian> {

    public CivilianReceivedVisitor() { }

    public override DelegatedSnapshotFactory Visit<TVisitedUser, TTransmission>(Received<TVisitedUser, TTransmission> received) {
      throw new System.NotImplementedException();
    }

  }
}
