using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Interfaces;
using Bot.Tools;

namespace Bot.Pipeline {
  public class ReceivedToSnapshot : IReceivedToSnapshot {

    private readonly List<IReceived<IUser, ITransmittable>> context = new List<IReceived<IUser, ITransmittable>>(); // Todo: Optimization: Use a circular buffer

    public ISnapshot<IUser, ITransmittable> GetSnapshot(IReceived<IUser, ITransmittable> first) {
      try {
        return new Snapshot<IUser, ITransmittable>(first, context.ToList()); // The .ToList() creates a new instance; important!
      } finally {
        context.Insert(0, first);
        if (context.Count > Settings.ContextSize) {
          context.RemoveAt(Settings.ContextSize);
        }
      }
    }

  }
}
