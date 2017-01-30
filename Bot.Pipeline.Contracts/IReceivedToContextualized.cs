using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface IReceivedToContextualized {
    IContextualized<IUser, ITransmittable> GetContextualized(IReceived<IUser, ITransmittable> received);
  }
}
