using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Tools.Interfaces;
using TwitchLib.Models.Client;

namespace Bot.Pipeline {
  public class TwitchMessageParser : IFactory<ChatMessage, IReceived<IUser, ITransmittable>> {
    private readonly ITimeService _timeService;

    public TwitchMessageParser(ITimeService timeService) {
      _timeService = timeService;
    }

    public IReceived<IUser, ITransmittable> Create(ChatMessage message) {
      if (message.IsModerator) {
        return new PublicMessageFromMod(message.DisplayName, message.Message, _timeService);
      } else {
        return new PublicMessageFromCivilian(message.DisplayName, message.Message, _timeService);
      }
    }

  }
}
