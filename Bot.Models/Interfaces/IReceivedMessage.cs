namespace Bot.Models.Interfaces {
  public interface IReceivedMessage<out TUser> : IReceived<TUser, IMessage>
    where TUser : IUser {
    string Text { get; }
  }
}
