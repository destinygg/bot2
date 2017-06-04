using System;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public class CommandHandlerTryCatchDecorator : ICommandHandler {
    private readonly ICommandHandler _decoratedHandler;
    private readonly ILogger _logger;

    public CommandHandlerTryCatchDecorator(ICommandHandler decoratedHandler, ILogger logger) {
      _decoratedHandler = decoratedHandler;
      _logger = logger;
      _logger.LogInformation($"{nameof(CommandHandlerTryCatchDecorator)} now decorates {_decoratedHandler.GetType()}");
    }

    public void Handle() {
      try {
        _decoratedHandler.Handle();
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(CommandHandlerTryCatchDecorator)} when handling {_decoratedHandler.GetType()}", e);
      }
    }
  }

  public class CommandHandlerTryCatchDecorator<TCommand> : ICommandHandler<TCommand> {
    private readonly ICommandHandler<TCommand> _decoratedHandler;
    private readonly ILogger _logger;

    public CommandHandlerTryCatchDecorator(ICommandHandler<TCommand> decoratedHandler, ILogger logger) {
      _decoratedHandler = decoratedHandler;
      _logger = logger;
      _logger.LogInformation($"{nameof(CommandHandlerTryCatchDecorator<object>)} now decorates {_decoratedHandler.GetType()}");
    }

    public void Handle(TCommand command) {
      try {
        _decoratedHandler.Handle(command);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(CommandHandlerTryCatchDecorator<object>)} when handling {_decoratedHandler.GetType()}", e);
      }
    }
  }

}
