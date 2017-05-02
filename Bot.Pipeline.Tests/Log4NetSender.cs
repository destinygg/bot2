using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using log4net;

namespace Bot.Pipeline.Tests {
  public class Log4NetSender : ICommandHandler<IEnumerable<ISendable<ITransmittable>>> {
    private readonly ISendableVisitor<string> _sendableVisitor;
    private readonly ILog _logger;

    public Log4NetSender(ISendableVisitor<string> sendableVisitor) {
      _sendableVisitor = sendableVisitor;
      _logger = LogManager.GetLogger(nameof(Log4NetSender));
    }

    public void Handle(IEnumerable<ISendable<ITransmittable>> sendables) {
      foreach (var sendable in sendables) {
        _logger.Info(sendable.Accept(_sendableVisitor));
      }
    }

  }
}
