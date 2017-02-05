using Bot.Models;
using Bot.Tools.Interfaces;

namespace Bot.Logic.ReceivedVisitor {
  public class ModeratorReceivedVisitor : BaseReceivedVisitor<Moderator> {

    public ModeratorReceivedVisitor(ILogger logger, ITimeService timeService) : base(logger, timeService) { }

    protected override DelegatedSnapshotFactory DynamicVisit(Received<Moderator, PublicMessage> received) =>
      NewSnapshotFactory(snapshot => new Snapshot<Moderator, PublicMessage>(received, snapshot));
  }
}
