using Bot.Models.Contracts;

namespace Bot.Pipeline.Interfaces {
  public interface IReceivedToSnapshot {
    ISnapshot<IUser, ITransmittable> GetSnapshot(IReceived<IUser, ITransmittable> received);
  }
}
