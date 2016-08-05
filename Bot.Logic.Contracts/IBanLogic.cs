using Bot.Models.Contracts;
using System.Collections.Generic;

namespace Bot.Logic.Contracts {
  public interface IBanLogic {
    IEnumerable<IBroadcast> Process(IPublicMessageReceived publicMessageReceived);
  }
}
