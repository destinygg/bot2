using System;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Tools;

namespace Bot.Logic.Tests.Helper
{
  public class ContextAppenderBuilder : ContextBuilder, IContextAppender {
    private int _appendedCount;
    private TimeSpan _cachedInterval;

    public ContextAppenderBuilder(TimeSpan timespan) {
      _cachedInterval = timespan;
    }

    private IReceived<IUser, ITransmittable> _ReceivedFactory(Func<string, IReceived<IUser, ITransmittable>> factory) => factory($"#{_appendedCount + 1}");

    public IContextAppender ModMessage(string message) {
      var received = _ReceivedFactory(nick => new PublicMessageFromMod(nick, message, NextTimestamp()));
      Nontargets.Add(received);
      _appendedCount++;
      return this;
    }

    public IContextAppender TargetedMessage(string message) {
      var received = _ReceivedFactory(nick => new PublicMessageFromCivilian(nick, message, NextTimestamp()));
      Targets.Add(received);
      _appendedCount++;
      return this;
    }

    public IContextAppender PublicMessage(string message) {
      var received = _ReceivedFactory(nick => new PublicMessageFromCivilian(nick, message, NextTimestamp()));
      Nontargets.Add(received);
      _appendedCount++;
      return this;
    }

    public IContextAppender TargetedMessage() => (this as ITransmissionBuilder<IContextAppender>).TargetedMessage("");
    public IContextAppender ModMessage() => (this as ITransmissionBuilder<IContextAppender>).ModMessage("");
    public IContextAppender PublicMessage() => (this as ITransmissionBuilder<IContextAppender>).PublicMessage("");

    public ITerminalAppender RadiusIs(string nukeBlastRadius) {
      base._nukeBlastRadius = TimeSpan.Parse(nukeBlastRadius);
      return this;
    }

    public DateTime NextTimestamp() {
      if (_cachedInterval <= TimeSpan.Zero)
        throw new ArgumentOutOfRangeException("_cachedInterval", "Interval is less than or equal to zero.");
      return DateTime.MinValue + _cachedInterval.Multiply(_appendedCount + 1);
    }

  }
}