namespace Bot.Models.Interfaces {
  public static class ISnapshotExtensionMethods {
    public static TUser Sender<TUser, TMessage>(this ISnapshot<TUser, TMessage> snapshot)
      where TUser : IUser
      where TMessage : ITransmittable =>
      snapshot.Latest.Sender;
  }
}
