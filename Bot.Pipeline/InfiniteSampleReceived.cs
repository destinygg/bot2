using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class InfiniteSampleReceived : ISampleReceived {

    public IEnumerable<IReceived> Receiveds {
      get {
        var rng = new Random();
        while (true) {
          yield return new PublicReceivedMessage("Hi!" + rng.Next(), DateTime.Now);
        }
      }
    }
  }
}
