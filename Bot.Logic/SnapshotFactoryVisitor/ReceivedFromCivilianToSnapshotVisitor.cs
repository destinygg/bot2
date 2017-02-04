using Bot.Models;
using Bot.Tools.Interfaces;

namespace Bot.Logic.SnapshotFactoryVisitor {
  public class ReceivedFromCivilianToSnapshotVisitor : UserVisitor<Civilian> {

    public ReceivedFromCivilianToSnapshotVisitor(ILogger logger, ITimeService timeService) : base(logger, timeService) { }

    protected override SnapshotFactory DynamicVisit(Received<Civilian, PublicMessage> received) =>
      NewSnapshotFactory(snapshot => new Snapshot<Civilian, PublicMessage>(received, snapshot));
  }
}
