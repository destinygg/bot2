using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class InfiniteSampleReceived : ISampleReceived {
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
