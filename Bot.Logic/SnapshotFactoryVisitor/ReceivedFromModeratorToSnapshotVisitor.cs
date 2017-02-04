using Bot.Models;
using Bot.Tools.Interfaces;

namespace Bot.Logic.SnapshotFactoryVisitor {
  public class ReceivedFromModeratorToSnapshotVisitor : UserVisitor<Moderator> {

    public ReceivedFromModeratorToSnapshotVisitor(ILogger logger, ITimeService timeService) : base(logger, timeService) { }

    protected override SnapshotFactory DynamicVisit(Received<Moderator, PublicMessage> received) =>
      NewSnapshotFactory(snapshot => new Snapshot<Moderator, PublicMessage>(received, snapshot));
  }
}
