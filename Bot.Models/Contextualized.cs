using System.Collections.Generic;
using System.Linq;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class Contextualized : IContextualized {

    private readonly IReadOnlyList<IReceived<IUser>> _allReceived;

    public Contextualized(IReadOnlyList<IReceived<IUser>> allReceived) {
      _allReceived = allReceived;
    }

    public IReceived<IUser> Latest => _allReceived.First();
    public IReadOnlyList<IReceived<IUser>> Context => _allReceived.Skip(1).ToList();

  }
}
