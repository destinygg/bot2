using System.Collections.Generic;
using System.Linq;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class Contextualized : IContextualized {

    private readonly IReadOnlyList<IReceived> _allReceived;

    public Contextualized(IReadOnlyList<IReceived> allReceived) {
      _allReceived = allReceived;
    }

    public IReceived First => _allReceived.First();
    public IReadOnlyList<IReceived> Context => _allReceived.Skip(1).ToList();

  }
}
