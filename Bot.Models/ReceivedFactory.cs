using System;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedFactory : IReceivedFactory {
    private readonly ITimeService _timeService;

    public ReceivedFactory(ITimeService timeService) {
      _timeService = timeService;
    }

    public ModPublicReceivedMessage ModPublicReceivedMessage(string text) => new ModPublicReceivedMessage(text, _timeService);
    public ModPublicReceivedMessage ModPublicReceivedMessage(string text, DateTime timestamp) => new ModPublicReceivedMessage(text, timestamp);

    public PublicReceivedMessage PublicReceivedMessage(string text) => new PublicReceivedMessage(text, _timeService);
    public PublicReceivedMessage PublicReceivedMessage(string text, DateTime timestamp) => new PublicReceivedMessage(text, timestamp);

    public ReceivedUnMuteBan ReceivedUnMuteBan(IUser sender, IUser target) => new ReceivedUnMuteBan(sender, target, _timeService);
  }
}
