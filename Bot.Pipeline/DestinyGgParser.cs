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
            return null;
            var join = _jsonParser.Create<Models.Websockets.ReceivedJoin.RootObject>(json);
            return join.features.Any(_isMod)
              ? new Models.Received.ReceivedJoin(new Moderator(join.nick), GetTimestamp(join.timestamp))
              : new Models.Received.ReceivedJoin(new Civilian(join.nick, join.features.All(_isProtected)), GetTimestamp(join.timestamp));
          }
        case "QUIT": {
            return null;
            var quit = _jsonParser.Create<Models.Websockets.ReceivedQuit.RootObject>(json);
            return quit.features.Any(_isMod)
              ? new Models.Received.ReceivedQuit(new Moderator(quit.nick), GetTimestamp(quit.timestamp))
              : new Models.Received.ReceivedQuit(new Civilian(quit.nick, quit.features.All(_isProtected)), GetTimestamp(quit.timestamp));
          }
        case "MSG": {
            var message = _jsonParser.Create<ReceivedMsg.RootObject>(json);
            return message.features.Any(_isMod)
              ? (IReceived<IUser, ITransmittable>) new PublicMessageFromMod(message.nick, message.data, GetTimestamp(message.timestamp))
              : (IReceived<IUser, ITransmittable>) new PublicMessageFromCivilian(message.nick, message.data, GetTimestamp(message.timestamp), message.features.All(_isProtected));
          }
        case "ERR": {
            switch (json.Trim('"')) {
              case "notfound":
              case "nopermission":
              case "needbanreason":
                return new ReceivedError($"Server reported error: {json}", _timeService);
              //case "duplicate":
              //case "protocolerror":
              default:
                throw new NotImplementedException($"{nameof(DestinyGgParser)}'s ERR did not have a case for {json}");
            }
          }
        case "REFRESH":
        case "MUTE":
        case "BAN":
        case "UNMUTE":
        case "UNBAN":
        case "BROADCAST": {
            /* 
               REFRESH {"nick":"Bot","features":["protected","subscriber","bot","flair8"],"timestamp":1494299684381}
               MUTE {"nick":"Bot","features":["protected","subscriber","bot","flair8"],"timestamp":1494300981192,"data":"dharmatest"}
               BAN {"nick":"Bot","features":["protected","subscriber","bot","flair8"],"timestamp":1494301117732,"data":"dharmatest"}
               UNMUTE {"nick":"Bot","features":["protected","subscriber","bot","flair8"],"timestamp":1494301248941,"data":"dharmatest"}
               UNBAN {"nick":"Bot","features":["protected","subscriber","bot","flair8"],"timestamp":1494301339617,"data":"dharmatest"}
               BROADCAST {"timestamp":1494301406689,"data":"dharmatest (sorry, this should be the last)"}
            */
            return null;
          }
      }
      throw new NotImplementedException($"{nameof(DestinyGgParser)} did not have a case for {input}");
    }

    private DateTime GetTimestamp(long timestamp) => TimeService.UnixEpoch.AddMilliseconds(timestamp);

    public IReceived<IUser, ITransmittable> OnErrorCreate => new ReceivedError($"An error occured in {nameof(DestinyGgParser)}", _timeService);
  }
}
