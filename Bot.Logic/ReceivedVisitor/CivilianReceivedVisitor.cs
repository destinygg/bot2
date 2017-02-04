using Bot.Models;
using Bot.Tools.Interfaces;

namespace Bot.Logic.ReceivedVisitor {
  public class CivilianReceivedVisitor : BaseReceivedVisitor<Civilian> {

    public CivilianReceivedVisitor(ILogger logger, ITimeService timeService) : base(logger, timeService) { }

    protected override SnapshotFactory DynamicVisit(Received<Civilian, PublicMessage> received) =>
      NewSnapshotFactory(snapshot => new Snapshot<Civilian, PublicMessage>(received, snapshot));
  }
}
