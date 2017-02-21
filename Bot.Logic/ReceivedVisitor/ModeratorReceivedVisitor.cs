using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.ReceivedVisitor {
  public class ModeratorReceivedVisitor : BaseReceivedVisitor<Moderator> {
    public override DelegatedSnapshotFactory Visit(IReceived<Civilian, PublicMessage> t) {
      throw new System.NotImplementedException();
    }

    public override DelegatedSnapshotFactory Visit(IReceived<Moderator, PublicMessage> t) {
      throw new System.NotImplementedException();
    }

    public override DelegatedSnapshotFactory Visit(IReceived<Moderator, ErrorMessage> t) {
      throw new System.NotImplementedException();
    }

    public override DelegatedSnapshotFactory Visit(IReceived<Moderator, Pardon> t) {
      throw new System.NotImplementedException();
    }

  }
}
