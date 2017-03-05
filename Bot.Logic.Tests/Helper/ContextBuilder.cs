using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;

namespace Bot.Logic.Tests.Helper {

  // Everything is in one class so state doesn't have to be passed around.
  // The code is relatively simple, but the design is not.
  // A state diagram is available at "ContextBuilder State Diagram.png"
  // If the design changes, update "ContextBuilder State Diagram.xml" using https://draw.io/

  public interface ITransmissionBuilder<out T> {
    T ModMessage(string message);
    T ModMessage(); // interfaces with optional parameters can be inconsistently implemented
    T TargetedMessage(string message);
    T TargetedMessage();
    T PublicMessage(string message);
    T PublicMessage();
  }

  public interface IContextInserter : ITransmissionBuilder<ITimeInserter> { }

  public interface IContextAppender : ITransmissionBuilder<IContextAppender>, ITerminalAppender {
    ITerminalAppender RadiusIs(string nukeBlastRadius);
  }

  public interface ITerminalAppender : ITerminalBuilder {
    DateTime NextTimestamp();
  }

  public interface ITerminalInserter : ITerminalBuilder {
    DateTime CreatedAt { get; }
  }

  public interface ITerminalBuilder {
    void VerifyTargeted(IEnumerable<IUser> expectedTargets);
    List<IReceived<IUser, ITransmittable>> Build();
    TimeSpan NukeBlastRadius { get; }
  }

  public interface ITimeInserter {
    IContextInserter InsertAt(string timestamp);
    IRadiusAndTerminalInserter CreateAt(string timestamp);
  }

  public interface IRadiusAndTerminalInserter : ITerminalBuilder {
    ITerminalInserter RadiusIs(string nukeBlastRadius);
  }

  public abstract class ContextBuilder {
    protected string CachedNick;
    protected readonly HashSet<string> Nicks = new HashSet<string>();
    protected readonly IList<IReceived<IUser, ITransmittable>> Nontargets = new List<IReceived<IUser, ITransmittable>>();
    protected readonly IList<IReceived<IUser, ITransmittable>> Targets = new List<IReceived<IUser, ITransmittable>>();
    private IReadOnlyList<string> _ActualTargeted => Targets.Select(r => r.Sender.Nick).ToList();

    protected TimeSpan? _nukeBlastRadius;
    public TimeSpan NukeBlastRadius => (TimeSpan) _nukeBlastRadius;

    public List<IReceived<IUser, ITransmittable>> Build() => Targets.Concat(Nontargets).OrderBy(r => r.Timestamp).ToList();

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
