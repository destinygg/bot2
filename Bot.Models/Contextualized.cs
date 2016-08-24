using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class Contextualized : IContextualized {
    public Contextualized(IReadOnlyList<IReceived> context) {
      Context = context;
    }

    public IReceived First => Context.First();
    public IReadOnlyList<IReceived> Context { get; }
  }
}
