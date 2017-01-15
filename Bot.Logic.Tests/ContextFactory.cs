using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic.Tests {
  /// <remarks>
  /// "Context" here differs from IContextualized in that IContextualized's context skips the first message.
  /// This Builder generates the list of IReceived that Contextualized takes in its ctor
  /// </remarks>>
  public class ContextBuilder {
    private DateTime _Time { get; set; } = _RootTime;
    private DateTime _RootTime { get; } = DateTime.UtcNow.Date;
    private readonly TimeSpan _gap;

    private IList<IReceived> _Nontargets { get; } = new List<IReceived>();
    private IList<IReceived> _Targets { get; } = new List<IReceived>();

    private IReadOnlyList<IUser> _NonTargetUsers => _Nontargets.Select(r => r.Sender).ToList();
    private IReadOnlyList<IUser> _TargetUsers => _Targets.Select(r => r.Sender).ToList();

    public ContextBuilder(TimeSpan gap) {
      _gap = gap;
    }

    public ContextBuilder() {
      _gap = TimeSpan.FromMinutes(1);
    }

    public IReadOnlyList<IReceived> GetContext => _Targets.Concat(_Nontargets).ToList(); // Is 1 indexed
    public bool IsValid(IReadOnlyList<IUser> targets) => targets.SequenceEqual(_TargetUsers) && !_NonTargetUsers.Intersect(targets).Any();

    public ContextBuilder TargetedMessage(string message, TimeSpan? timestamp = null)
      => AddReceived(timestamp, () => _Targets.Add(new PublicReceivedMessage(message, _Time)));

    public ContextBuilder PublicMessage(string message, TimeSpan? timestamp = null)
      => AddReceived(timestamp, () => _Nontargets.Add(new PublicReceivedMessage(message, _Time)));

    public ContextBuilder ModMessage(string message, TimeSpan? timestamp = null)
      => AddReceived(timestamp, () => _Nontargets.Add(new ModPublicReceivedMessage(message, _Time)));

    private ContextBuilder AddReceived(TimeSpan? timestamp, Action addReceived) {
      _Time = timestamp == null ? _Time.Add(_gap) : _RootTime.Add((TimeSpan) timestamp);
      addReceived.Invoke();
      return this;
    }

  }
}
