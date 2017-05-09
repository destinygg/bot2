using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Models.Websockets;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class DestinyGgParser : IErrorableFactory<string, IReceived<IUser, ITransmittable>> {

    private readonly IGenericClassFactory<string> _jsonParser;
    private readonly ITimeService _timeService;
    private readonly Func<string, bool> _isProtected = f => f != "protected";
    private readonly Func<string, bool> _isMod = f => f == "bot" || f == "admin" || f == "moderator";

    public DestinyGgParser(IGenericClassFactory<string> jsonParser, ITimeService timeService) {
      _jsonParser = jsonParser;
      _timeService = timeService;
    }

    public IReceived<IUser, ITransmittable> Create(string input) {
      var index = input.IndexOf(' ');
      var command = input.Substring(0, index);
      var json = input.Substring(index + 1);
      switch (command) {
        case "NAMES": {
            var receivedNames = _jsonParser.Create<ReceivedNames.RootObject>(json);
            var mods = receivedNames.users.Where(u => u.features.Any(_isMod)).Select(u => new Moderator(u.nick));
            var civilians = receivedNames.users.Where(u => !u.features.Any(_isMod)).Select(u => new Civilian(u.nick, u.features.All(_isProtected)));
            var initialUsers = new InitialUsers(new List<IUser>().Concat(mods).Concat(civilians));
            return new ReceivedInitialUsers(_timeService.UtcNow, initialUsers);
          }
        case "JOIN": {
            var join = _jsonParser.Create<Models.Websockets.ReceivedJoin.RootObject>(json);
            return join.features.Any(_isMod)
              ? new Models.Received.ReceivedJoin(new Moderator(join.nick), GetTimestamp(join.timestamp))
              : new Models.Received.ReceivedJoin(new Civilian(join.nick, join.features.All(_isProtected)), GetTimestamp(join.timestamp));
          }
        case "MSG": {
            var message = _jsonParser.Create<ReceivedMsg.RootObject>(json);
            return message.features.Any(_isMod)
              ? (IReceived<IUser, ITransmittable>) new PublicMessageFromMod(message.nick, message.data, GetTimestamp(message.timestamp))
              : (IReceived<IUser, ITransmittable>) new PublicMessageFromCivilian(message.nick, message.data, GetTimestamp(message.timestamp), message.features.All(_isProtected));
          }
      }
      return null;
    }

    private DateTime GetTimestamp(long timestamp) => TimeService.UnixEpoch.AddMilliseconds(timestamp);

    public IReceived<IUser, ITransmittable> OnErrorCreate => new ReceivedError($"An error occured in {nameof(DestinyGgParser)}", _timeService);
  }
}
