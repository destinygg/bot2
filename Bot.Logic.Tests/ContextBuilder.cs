using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;
using Bot.Models.Received;

namespace Bot.Logic.Tests {

  public interface IContextInserter {
    IContextTimeSetter ModMessage(string message);
    IContextTimeSetter ModMessage(); // interfaces with optional parameters can be inconsistently implemented
    IContextTimeSetter TargetedMessage(string message);
    IContextTimeSetter TargetedMessage();
    IContextTimeSetter PublicMessage(string message);
    IContextTimeSetter PublicMessage();
  }

  public interface IContextTimeSetter {
    IContextInserter InsertAt(string timestamp);
    IReadOnlyList<IReceived<IUser, ITransmittable>> Build();
  }

  public class ContextBuilder : IContextTimeSetter, IContextInserter {
    private DateTime _cachedTime;
    private string _cachedNick;
    private readonly HashSet<string> _nicks = new HashSet<string>();
    private readonly IList<IReceived<IUser, ITransmittable>> _nontargets = new List<IReceived<IUser, ITransmittable>>();
    private readonly IList<IReceived<IUser, ITransmittable>> _targets = new List<IReceived<IUser, ITransmittable>>();
    private IReadOnlyList<string> _ActualTargeted => _targets.Select(r => r.Sender.Nick).ToList();

    #region IContextTransmissionBuilder
    public IContextTimeSetter ModMessage(string message) {
      _nontargets.Add(new PublicMessageFromMod(_cachedNick, message, _cachedTime));
      return this;
    }

    public IContextTimeSetter TargetedMessage(string message) {
      _targets.Add(new PublicMessageFromCivilian(_cachedNick, message, _cachedTime));
      return this;
    }

    public IContextTimeSetter PublicMessage(string message) {
      _nontargets.Add(new PublicMessageFromCivilian(_cachedNick, message, _cachedTime));
      return this;
    }

    public IContextTimeSetter ModMessage() => ModMessage("");
    public IContextTimeSetter TargetedMessage() => TargetedMessage("");
    public IContextTimeSetter PublicMessage() => PublicMessage("");
    #endregion

    #region IContextTimeBuilder
    public IContextInserter InsertAt(string timestamp) {
      _cachedTime = TimeParser.Parse(timestamp);
      _cachedNick = timestamp;
      if (_nicks.Contains(_cachedNick)) {
        throw new Exception("Nicks/timestamps must be unique. If you want messages with the same timestamp, zero pad them.");
      }
      _nicks.Add(_cachedNick);
      return this;
    }

    public IReadOnlyList<IReceived<IUser, ITransmittable>> Build() =>
      _targets.Concat(_nontargets).OrderBy(r => r.Timestamp).ToList();
    #endregion

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
