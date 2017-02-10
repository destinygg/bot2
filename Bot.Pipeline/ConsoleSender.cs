using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;

namespace Bot.Pipeline {
  public class ConsoleSender : ICommandHandler<IEnumerable<ISendable<ITransmittable>>> {
    private readonly ISendableVisitor<string> _sendableVisitor;

    public ConsoleSender(ISendableVisitor<string> sendableVisitor) {
      _sendableVisitor = sendableVisitor;
    }

    public void Handle(IEnumerable<ISendable<ITransmittable>> sendables) {
      foreach (var sendable in sendables) {
        Console.WriteLine(sendable.Accept(_sendableVisitor));
      }
    }

  }
}
