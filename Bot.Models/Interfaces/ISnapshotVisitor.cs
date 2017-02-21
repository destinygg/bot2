namespace Bot.Models.Interfaces {
  public interface ISnapshotVisitor<out TResult> {
    TResult Visit<TUser, TTransmission>(ISnapshot<TUser, TTransmission> received)
      where TUser : IUser
      where TTransmission : ITransmittable;
  }
}
