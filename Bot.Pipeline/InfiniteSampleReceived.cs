using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class InfiniteSampleReceived {
    private readonly ITimeService _timeService;

    public InfiniteSampleReceived(ITimeService timeService) {
      _timeService = timeService;
    }

    public IEnumerable<IReceived<IUser, ITransmittable>> Receiveds {
      get {
        var rng = new Random();
        while (true) {
          yield return new PublicMessageFromCivilian("Hi!" + rng.Next(), _timeService.UtcNow);
        }
      }
    }

  }
}
