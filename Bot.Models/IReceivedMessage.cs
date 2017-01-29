using Bot.Models.Contracts;

namespace Bot.Models {
  public interface IReceivedMessage<out T> : IReceived<T>, IMessage
    where T : IUser {

  }
}
