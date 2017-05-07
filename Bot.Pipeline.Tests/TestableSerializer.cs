using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline.Tests {
  public class TestableSerializer : IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>> {
    private readonly Action<string> _action;

    public TestableSerializer(Action<string> action = null) {
      _action = action;
    }

    public IEnumerable<string> Create(IEnumerable<ISendable<ITransmittable>> sendables) {
      foreach (var sendable in sendables) {
        Console.WriteLine(sendable);
        Outbox.Add(sendable);
        _action?.Invoke(sendable.ToString());
        yield return sendable.ToString();
      }
    }

    public IList<ISendable<ITransmittable>> Outbox { get; } = new List<ISendable<ITransmittable>>();
  }
}
