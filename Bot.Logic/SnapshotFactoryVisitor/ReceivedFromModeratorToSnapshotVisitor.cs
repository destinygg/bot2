using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SnapshotFactoryVisitor {
  public class ReceivedFromModeratorToSnapshotVisitor : IReceivedVisitor<SnapshotFactory> {
    public SnapshotFactory Visit<TUser, TTransmission>(Received<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable =>
      _DynamicVisit(received as dynamic);

    private SnapshotFactory _DynamicVisit(Received<Moderator, PublicMessage> received) =>
      new SnapshotFactory(snapshot => new Snapshot<Moderator, PublicMessage>(received, snapshot));
  }
}
