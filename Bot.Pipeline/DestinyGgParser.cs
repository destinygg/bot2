using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Models.Websockets;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class DestinyGgParser : IErrorableFactory<string, IReceived<IUser, ITransmittable>> {

    private readonly IGenericClassFactory<string> _jsonParser;
    private readonly ITimeService _timeService;

    public DestinyGgParser(IGenericClassFactory<string> jsonParser, ITimeService timeService) {
      _jsonParser = jsonParser;
      _timeService = timeService;
    }

    public IReceived<IUser, ITransmittable> Create(string input) {
      var index = input.IndexOf(' ');
      var command = input.Substring(0, index);
      var json = input.Substring(index + 1);
      if (command == "NAMES") {
        var receivedNames = _jsonParser.Create<ReceivedNames.RootObject>(json);
        var mods = receivedNames.users.Where(u => u.features.Any(f => f == "bot" || f == "admin" || f == "moderator")).Select(u => new Moderator(u.nick)).ToList();
        var civilians = receivedNames.users.Where(u => !u.features.Any(f => f == "bot" || f == "admin" || f == "moderator")).Select(u => new Civilian(u.nick, u.features.All(f => f != "protected"))).ToList().OrderBy(x => x.Nick);
        var initialUsers = new InitialUsers(new List<IUser>().Concat(mods).Concat(civilians));
        return new ReceivedInitialUsers(_timeService.UtcNow, initialUsers);
      }
      return null;
    }

    public IReceived<IUser, ITransmittable> OnErrorCreate => new ReceivedError($"An error occured in {nameof(DestinyGgParser)}", _timeService);
  }
}
