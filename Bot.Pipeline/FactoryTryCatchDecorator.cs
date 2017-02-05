using System;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class FactoryTryCatchDecorator<T, TOutput> : IFactory<T, TOutput> {
    private readonly IFactory<T, TOutput> _factory;
    private readonly ILogger _logger;

    public FactoryTryCatchDecorator(IFactory<T, TOutput> factory, ILogger logger) {
      _factory = factory;
      _logger = logger;
    }

    public TOutput Create(T input) {
      try {
        return _factory.Create(input);
      } catch (Exception e) {
        _logger.LogError(e, $"Error occured in {nameof(FactoryTryCatchDecorator<object, object>)}");
        throw;
      }
    }

  }
}
