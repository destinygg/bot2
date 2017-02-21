using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.ReceivedVisitor {
  public class ReceivedVisitor : IReceivedVisitor<DelegatedSnapshotFactory> {
    public DelegatedSnapshotFactory Visit(IReceived<Civilian, PublicMessage> t) {
      throw new System.NotImplementedException();
    }

    public DelegatedSnapshotFactory Visit(IReceived<Moderator, PublicMessage> t) {
      throw new System.NotImplementedException();
    }

    public DelegatedSnapshotFactory Visit(IReceived<Moderator, ErrorMessage> t) {
      throw new System.NotImplementedException();
    }

    public DelegatedSnapshotFactory Visit(IReceived<Moderator, Pardon> t) {
      throw new System.NotImplementedException();
    }
  }
}
