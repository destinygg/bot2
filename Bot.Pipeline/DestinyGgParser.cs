using System;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class DestinyGgParser : IErrorableFactory<string, IReceived<IUser, ITransmittable>> {

    private readonly ITimeService _timeService;

    public DestinyGgParser(ITimeService timeService) {
      _timeService = timeService;
    }

    public IReceived<IUser, ITransmittable> Create(string input) {
      throw new NotImplementedException();
    }

    public IReceived<IUser, ITransmittable> OnErrorCreate => new ReceivedError($"An error occured in {nameof(DestinyGgParser)}", _timeService);
  }
}
