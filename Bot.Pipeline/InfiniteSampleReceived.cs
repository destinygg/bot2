using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Pipeline {
  public class InfiniteSampleReceived : ISampleReceived {
    private readonly ITimeService _timeService;

    public InfiniteSampleReceived(ITimeService timeService) {
      _timeService = timeService;
    }

    public IEnumerable<IReceived> Receiveds {
      get {
        var rng = new Random();
        while (true) {
          yield return new PublicMessageFromCivilian("Hi!" + rng.Next(), _timeService.UtcNow);
        }
      }
    }
  }
}
