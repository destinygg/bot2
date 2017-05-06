using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class CommandLogic : ICommandLogic {
    private readonly ITimeService _timeService;

    public CommandLogic(ITimeService timeService) {
      _timeService = timeService;
    }

    public ISendable<PublicMessage> Time() => new SendablePublicMessage($"{_timeService.DestinyNow.ToShortTimeString()} Central Steven Time");
  }
}
