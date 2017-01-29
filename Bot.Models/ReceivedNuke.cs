using System;
using Bot.Models.Contracts;
using Bot.Tools;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public abstract class ReceivedNuke : IReceivedNuke {
    private readonly ITimeService _timeService;

    protected ReceivedNuke(ReceivedMessage<Moderator> message, ITimeService timeService) {
      _timeService = timeService;
      Timestamp = message.Timestamp;
      Sender = message.Sender;
    }

    public bool WillPunish<T>(T message) where T : IReceived<IUser>, IMessage =>
      MatchesNukedTerm(message.Text) &&
      WithinRange(message) &&
      !_IsExpired(message);

    private bool WithinRange<T>(T message) where T : IReceived<IUser> =>
      message.Timestamp.IsWithin(Timestamp, Settings.NukeBlastRadius);

    private bool _IsExpired(IReceived<IUser> message) {
      var punishmentTimestamp = message.Timestamp <= Timestamp ? Timestamp : message.Timestamp;
      var expirationDate = punishmentTimestamp + Duration;
      return expirationDate < _timeService.UtcNow;
    }

    protected abstract bool MatchesNukedTerm(string possibleVictimText);
    public abstract TimeSpan Duration { get; }
    public DateTime Timestamp { get; }
    public IUser Sender { get; }

  }
}
