using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic.Tests {
  /// <remarks>
  /// "Context" here differs from IContextualized in that IContextualized's context skips the first message.
  /// This Builder generates the list of IReceived that Contextualized takes in its ctor
  /// </remarks>>
  public class ContextBuilder {
    private DateTime _time = DateTime.Today.AddMinutes(1); // 1 indexed
    private readonly List<IReceived> _nontargets = new List<IReceived>();
    private readonly List<IReceived> _targets = new List<IReceived>();
    private IReadOnlyList<IUser> _NonTargetUsers => _nontargets.Select(r => r.Sender).ToList();
    private IReadOnlyList<IUser> _TargetUsers => _targets.Select(r => r.Sender).ToList();
    public IReadOnlyList<IReceived> GetContext => _targets.Concat(_nontargets).ToList();

    public ContextBuilder PublicMessage(string message) {
      _nontargets.Add(new PublicReceivedMessage(message, _time));
      _time = _time.AddMinutes(1);
      return this;
    }

    public ContextBuilder TargetedMessage(string message) {
      var prm = new PublicReceivedMessage(message, _time);
      _targets.Add(prm);
      _time = _time.AddMinutes(1);
      return this;
    }

    public ContextBuilder ModMessage(string message) {
      _nontargets.Add(new ModPublicReceivedMessage(message, _time));
      _time = _time.AddMinutes(1);
      return this;
    }

    public bool IsValid(IReadOnlyList<IUser> targets) => targets.SequenceEqual(_TargetUsers) && !_NonTargetUsers.Intersect(targets).Any();
  }
}
