using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Snapshot;

namespace Bot.Logic.ReceivedVisitor {
  public class ReceivedVisitor : IReceivedVisitor<DelegatedSnapshotFactory> {
    public DelegatedSnapshotFactory Visit(IReceived<Civilian, PublicMessage> message) =>
      new DelegatedSnapshotFactory(context => new Snapshot<Civilian, PublicMessage>(message, context));

    public DelegatedSnapshotFactory Visit(IReceived<Moderator, PublicMessage> message) =>
      new DelegatedSnapshotFactory(context => new Snapshot<Moderator, PublicMessage>(message, context));

    public DelegatedSnapshotFactory Visit(IReceived<Moderator, ErrorMessage> message) =>
      new DelegatedSnapshotFactory(context => new Snapshot<Moderator, ErrorMessage>(message, context));

    public DelegatedSnapshotFactory Visit(IReceived<Moderator, Pardon> pardon) =>
      new DelegatedSnapshotFactory(context => new Snapshot<Moderator, Pardon>(pardon, context));

  }
}
