using System;
using Bot.Models.Received;
using Bot.Tests;

namespace Bot.Logic.Tests.Helper {
  public class ContextInserterBuilder : ContextBuilder, IContextInserter, ITimeInserter, ITerminalInserter, IRadiusAndTerminalInserter {
    private DateTime _cachedTime = DateTime.MinValue;

    public IContextInserter InsertAt(string timestamp) {
      _cachedTime = TestHelper.Parse(timestamp);
      CachedNick = timestamp;
      if (Nicks.Contains(CachedNick)) {
        throw new Exception("Nicks/timestamps must be unique. If you want messages with the same timestamp, zero pad them.");
      }
      Nicks.Add(CachedNick);
      return this;
    }

    ITimeInserter ITransmissionBuilder<ITimeInserter>.ModMessage(string message) {
      Nontargets.Add(new PublicMessageFromMod(CachedNick, message, _cachedTime));
      return this;
    }

    ITimeInserter ITransmissionBuilder<ITimeInserter>.TargetedMessage(string message) {
      Targets.Add(new PublicMessageFromCivilian(CachedNick, message, _cachedTime));
      return this;
    }

    ITimeInserter ITransmissionBuilder<ITimeInserter>.PublicMessage(string message) {
      Nontargets.Add(new PublicMessageFromCivilian(CachedNick, message, _cachedTime));
      return this;
    }

    public ITimeInserter ModMessage() => (this as ITransmissionBuilder<ITimeInserter>).ModMessage("");
    public ITimeInserter TargetedMessage() => (this as ITransmissionBuilder<ITimeInserter>).TargetedMessage("");
    public ITimeInserter PublicMessage() => (this as ITransmissionBuilder<ITimeInserter>).PublicMessage("");

    private DateTime? _builtAt;
    public DateTime CreatedAt => (DateTime) _builtAt;
    IRadiusAndTerminalInserter ITimeInserter.CreateAt(string buildAt) {
      _builtAt = TestHelper.Parse(buildAt);
      return this;
    }

    public ITerminalInserter RadiusIs(string nukeBlastRadius) {
      _nukeBlastRadius = TimeSpan.Parse(nukeBlastRadius);
      return this;
    }

  }
}
