using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;
using Bot.Tools;

namespace Bot.Pipeline {
  public class ReceivedToContextualized : IReceivedToContextualized {

    private readonly List<IReceived> _allReceived = new List<IReceived>(); // Todo: Optimization: Use a circular buffer

    public IContextualized GetContextualized(IReceived first) {
      _allReceived.Insert(0, first);
      if (_allReceived.Count > Settings.ContextSize) {
        _allReceived.RemoveAt(Settings.ContextSize);
      }
      return new Contextualized(_allReceived.ToList()); // The .ToList() creates a new instance; important!
    }

  }
}
