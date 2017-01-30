using Bot.Models.Contracts;

namespace Bot.Models {
  public interface IReceivedMessage<out TUser> : IReceived<TUser, IMessage>
    where TUser : IUser {
    string Text { get; }
  }
}
