using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;

namespace Bot.Pipeline.Tests {
  public class TestableSender : ICommandHandler<IEnumerable<ISendable<ITransmittable>>> {
    private readonly Action<string> _action;

    public TestableSender(Action<string> action = null) {
      _action = action;
    }

    public void Handle(IEnumerable<ISendable<ITransmittable>> sendables) {
      foreach (var sendable in sendables) {
        Console.WriteLine(sendable);
        Outbox.Add(sendable);
        _action?.Invoke(sendable.ToString());
      }
    }

    public IList<ISendable<ITransmittable>> Outbox { get; } = new List<ISendable<ITransmittable>>();
  }
}
