using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.SnapshotFactoryVisitor {
  public class ReceivedFromCivilianToSnapshotVisitor : IReceivedVisitor<SnapshotFactory> {
    public SnapshotFactory Visit<TUser, TTransmission>(Received<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable =>
      _DynamicVisit(received as dynamic);

    private SnapshotFactory _DynamicVisit(Received<Civilian, PublicMessage> received) =>
      new SnapshotFactory(snapshot => new Snapshot<Civilian, PublicMessage>(received, snapshot));
  }
}
