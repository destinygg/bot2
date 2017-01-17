using System;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedFactory : IReceivedFactory {
    private readonly ITimeService _timeService;
    private readonly IModCommandParser _modCommandParser;

    public ReceivedFactory(ITimeService timeService, IModCommandParser modCommandParser) {
      _timeService = timeService;
      _modCommandParser = modCommandParser;
    }

    public ModPublicReceivedMessage ModPublicReceivedMessage(string text) => new ModPublicReceivedMessage(text, _timeService);
    public ModPublicReceivedMessage ModPublicReceivedMessage(string text, DateTime timestamp) => new ModPublicReceivedMessage(text, timestamp);

    public PublicReceivedMessage PublicReceivedMessage(string text) => new PublicReceivedMessage(text, _timeService);
    public PublicReceivedMessage PublicReceivedMessage(string text, DateTime timestamp) => new PublicReceivedMessage(text, timestamp);

    public ReceivedUnMuteBan ReceivedUnMuteBan(IUser sender, IUser target) => new ReceivedUnMuteBan(sender, target, _timeService);

    public ReceivedRegexNuke ReceivedRegexNuke(ReceivedMessage message) => new ReceivedRegexNuke(message, _modCommandParser, _timeService);
    public ReceivedStringNuke ReceivedStringNuke(ReceivedMessage message) => new ReceivedStringNuke(message, _modCommandParser, _timeService);

  }
}
