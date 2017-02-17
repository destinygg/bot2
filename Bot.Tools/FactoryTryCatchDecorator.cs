using System;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Tools {
  public class FactoryTryCatchDecorator<T, TOutput> : IErrorableFactory<T, TOutput> {
    private readonly IErrorableFactory<T, TOutput> _factory;
    private readonly ILogger _logger;

    public FactoryTryCatchDecorator(IErrorableFactory<T, TOutput> factory, ILogger logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(FactoryTryCatchDecorator<object, object>)} now decorates {_factory.GetType()}");
    }

    public TOutput Create(T input) {
      try {
        return _factory.Create(input);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(FactoryTryCatchDecorator<object, object>)}", e);
        return _factory.OnErrorCreate;
      }
    }

    public TOutput OnErrorCreate => _factory.OnErrorCreate;
  }
}
