using System.Collections.Generic;
using System.Linq;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class Contextualized : IContextualized {

    private readonly IReadOnlyList<IReceived<IUser, ITransmittable>> _allReceived;

    public Contextualized(IReadOnlyList<IReceived<IUser, ITransmittable>> allReceived) {
      _allReceived = allReceived;
    }

    public IReceived<IUser, ITransmittable> Latest => _allReceived.First();
    public IReadOnlyList<IReceived<IUser, ITransmittable>> Context => _allReceived.Skip(1).ToList();

  }
}
