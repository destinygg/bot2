using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;
using Bot.Tools;

namespace Bot.Logic.Tests {
  /// <remarks>
  /// "Context" here differs from ISnapshot in that ISnapshot's context skips the first message.
  /// </remarks>>
  public class ContextBuilder {
    private readonly DateTime _rootTime = TimeService.UnixEpoch;
    private DateTime _time;

    private readonly IList<IReceived<IUser, ITransmittable>> _nontargets = new List<IReceived<IUser, ITransmittable>>();
    private readonly IList<IReceived<IUser, ITransmittable>> _targets = new List<IReceived<IUser, ITransmittable>>();

    private IReadOnlyList<IUser> _NonTargetUsers => _nontargets.Select(r => r.Sender).ToList();
    private IReadOnlyList<IUser> _TargetUsers => _targets.Select(r => r.Sender).ToList();

    public ContextBuilder() {
      _time = _rootTime;
    }

    public TimeSpan Gap { get; } = TimeSpan.FromMinutes(1);

    public IReadOnlyList<IReceived<IUser, ITransmittable>> GetContext => _targets.Concat(_nontargets).OrderBy(r => r.Timestamp).ToList(); // Is 1 indexed

    public bool IsValid(IReadOnlyList<IUser> targets)
      => targets.OrderBy(u => u.Nick).SequenceEqual(_TargetUsers.OrderBy(u => u.Nick)) && !_NonTargetUsers.Intersect(targets).Any();

    public ContextBuilder TargetedMessage(string message, TimeSpan? timestamp = null)
      => _AddReceived(timestamp, t => _targets.Add(new PublicMessageFromCivilian(message, t)));

    public ContextBuilder PublicMessage(string message, TimeSpan? timestamp = null)
      => _AddReceived(timestamp, t => _nontargets.Add(new PublicMessageFromCivilian(message, t)));

    public ContextBuilder ModMessage(string message, TimeSpan? timestamp = null)
      => _AddReceived(timestamp, t => _nontargets.Add(new PublicMessageFromMod(message, t)));

    private ContextBuilder _AddReceived(TimeSpan? timestamp, Action<DateTime> addReceived) {
      _time = timestamp == null ? _time.Add(Gap) : _rootTime.Add((TimeSpan) timestamp);
      addReceived.Invoke(_time);
      return this;
    }

    public ContextBuilder TargetedMessage(string message, string timestamp)
      => TargetedMessage(message, _ParseExact(timestamp));

    public ContextBuilder PublicMessage(string message, string timestamp)
      => PublicMessage(message, _ParseExact(timestamp));

    public ContextBuilder ModMessage(string message, string timestamp)
      => ModMessage(message, _ParseExact(timestamp));

    private TimeSpan _ParseExact(string timestamp) =>
      TimeSpan.ParseExact(timestamp, "g", CultureInfo.CurrentCulture);

    private DateTime _zerothReceivedTimestamp;
    public ContextBuilder SetTimestampOfZerothReceived(TimeSpan timestamp) {
      _zerothReceivedTimestamp = _rootTime.Add(timestamp);
      return this;
    }

    public DateTime GetTimestampOfZerothReceived => _zerothReceivedTimestamp == DateTime.MinValue ? GetContext.Last().Timestamp : _zerothReceivedTimestamp;
  }
}
