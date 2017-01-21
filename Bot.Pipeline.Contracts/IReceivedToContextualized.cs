using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface IReceivedToContextualized {
    IContextualized GetContextualized(IReceived received);
  }
}
