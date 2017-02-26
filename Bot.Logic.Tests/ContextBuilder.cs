using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Tools;

namespace Bot.Logic.Tests {

  public interface ITransmissionBuilder<out T> {
    T ModMessage(string message);
    T ModMessage(); // interfaces with optional parameters can be inconsistently implemented
    T TargetedMessage(string message);
    T TargetedMessage();
    T PublicMessage(string message);
    T PublicMessage();
  }

  public interface IContextInserter : ITransmissionBuilder<IContextTimeSetter> { }

  public interface IContextAppender : ITransmissionBuilder<IContextAppender> {
    IReadOnlyList<IReceived<IUser, ITransmittable>> Build();
    DateTime NextTimestamp { get; }
  }

  public interface IContextTimeSetter {
    IContextInserter InsertAt(string timestamp);
    IContextAppender SubsequentlySpacedBy(TimeSpan timespan);
    IReadOnlyList<IReceived<IUser, ITransmittable>> Build();
  }

  public class ContextBuilder : IContextTimeSetter, IContextInserter, IContextAppender {
    private DateTime _cachedTime = DateTime.MinValue;
    private TimeSpan _cachedInterval;
    private int _appendedCount;
    private string _cachedNick;
    private readonly HashSet<string> _nicks = new HashSet<string>();
    private readonly IList<IReceived<IUser, ITransmittable>> _nontargets = new List<IReceived<IUser, ITransmittable>>();
    private readonly IList<IReceived<IUser, ITransmittable>> _targets = new List<IReceived<IUser, ITransmittable>>();
    private IReadOnlyList<string> _ActualTargeted => _targets.Select(r => r.Sender.Nick).ToList();

    #region IContextTimeSetter
    public IContextInserter InsertAt(string timestamp) {
      _cachedTime = TimeParser.Parse(timestamp);
      _cachedNick = timestamp;
      if (_nicks.Contains(_cachedNick)) {
        throw new Exception("Nicks/timestamps must be unique. If you want messages with the same timestamp, zero pad them.");
      }
      _nicks.Add(_cachedNick);
      return this;
    }

    public IContextAppender SubsequentlySpacedBy(TimeSpan timespan) {
      _cachedTime = Build().LastOrDefault()?.Timestamp ?? DateTime.MinValue;
      _cachedInterval = timespan;
      return this;
    }
    #endregion

    #region IContextInserter
    IContextTimeSetter ITransmissionBuilder<IContextTimeSetter>.ModMessage(string message) {
      _nontargets.Add(new PublicMessageFromMod(_cachedNick, message, _cachedTime));
      return this;
    }

    IContextTimeSetter ITransmissionBuilder<IContextTimeSetter>.TargetedMessage(string message) {
      _targets.Add(new PublicMessageFromCivilian(_cachedNick, message, _cachedTime));
      return this;
    }

    IContextTimeSetter ITransmissionBuilder<IContextTimeSetter>.PublicMessage(string message) {
      _nontargets.Add(new PublicMessageFromCivilian(_cachedNick, message, _cachedTime));
      return this;
    }

    public IContextTimeSetter ModMessage() => (this as ITransmissionBuilder<IContextTimeSetter>).ModMessage("");
    public IContextTimeSetter TargetedMessage() => (this as ITransmissionBuilder<IContextTimeSetter>).TargetedMessage("");
    public IContextTimeSetter PublicMessage() => (this as ITransmissionBuilder<IContextTimeSetter>).PublicMessage("");
    #endregion

    #region IContextAppender
    private IReceived<IUser, ITransmittable> _ReceivedFactory(Func<string, IReceived<IUser, ITransmittable>> factory) => factory($"#{_appendedCount + 1}");

    IContextAppender ITransmissionBuilder<IContextAppender>.ModMessage(string message) {
      var received = _ReceivedFactory(nick => new PublicMessageFromMod(nick, message, NextTimestamp));
      _nontargets.Add(received);
      _appendedCount++;
      return this;
    }

    IContextAppender ITransmissionBuilder<IContextAppender>.TargetedMessage(string message) {
      var received = _ReceivedFactory(nick => new PublicMessageFromCivilian(nick, message, NextTimestamp));
      _targets.Add(received);
      _appendedCount++;
      return this;
    }

    IContextAppender ITransmissionBuilder<IContextAppender>.PublicMessage(string message) {
      var received = _ReceivedFactory(nick => new PublicMessageFromCivilian(nick, message, NextTimestamp));
      _nontargets.Add(received);
      _appendedCount++;
      return this;
    }

    IContextAppender ITransmissionBuilder<IContextAppender>.TargetedMessage() => (this as ITransmissionBuilder<IContextAppender>).TargetedMessage("");
    IContextAppender ITransmissionBuilder<IContextAppender>.ModMessage() => (this as ITransmissionBuilder<IContextAppender>).ModMessage("");
    IContextAppender ITransmissionBuilder<IContextAppender>.PublicMessage() => (this as ITransmissionBuilder<IContextAppender>).PublicMessage("");
    public DateTime NextTimestamp => _cachedTime + _cachedInterval.Multiply(_appendedCount + 1);
    #endregion

    public IReadOnlyList<IReceived<IUser, ITransmittable>> Build() => _targets.Concat(_nontargets).OrderBy(r => r.Timestamp).ToList();

    public void VerifyTargeted(IEnumerable<IUser> expectedTargets) {
      var sortedExpected = expectedTargets.Select(x => x.Nick).OrderBy(u => u).ToList();
      var sortedActual = _ActualTargeted.OrderBy(u => u).ToList();
      if (!sortedExpected.SequenceEqual(sortedActual)) {
        Console.WriteLine("Expected targets:" + string.Join(", ", sortedExpected));
        Console.WriteLine("Actual targets:" + string.Join(", ", sortedActual));
        throw new Exception("Expected targets are not equal to actual targets.");
      }
    }

  }
}
