using System;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Tools {
  public class ErrorableFactoryTryCatchDecorator<T, TOutput> : FactoryTryCatchDecorator<T, TOutput>, IErrorableFactory<T, TOutput> {
    private readonly IErrorableFactory<T, TOutput> _factory;
    private readonly ILogger _logger;

    public ErrorableFactoryTryCatchDecorator(IErrorableFactory<T, TOutput> factory, ILogger logger) : base(factory, logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(ErrorableFactoryTryCatchDecorator<object, object>)} now decorates {_factory.GetType()}");
    }

    public override TOutput Create(T input) {
      try {
        return _factory.Create(input);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(ErrorableFactoryTryCatchDecorator<object, object>)}\r\n" +
                         $"{LogExtraInformation(input)}", e);
        return _factory.OnErrorCreate;
      }
    }

    public TOutput OnErrorCreate => _factory.OnErrorCreate;
  }

  public class ErrorableFactoryTryCatchDecorator<T1, T2, TOutput> : FactoryTryCatchDecorator<T1, T2, TOutput>, IErrorableFactory<T1, T2, TOutput> {
    private readonly IErrorableFactory<T1, T2, TOutput> _factory;
    private readonly ILogger _logger;

    public ErrorableFactoryTryCatchDecorator(IErrorableFactory<T1, T2, TOutput> factory, ILogger logger) : base(factory, logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(ErrorableFactoryTryCatchDecorator<object, object, object>)} now decorates {_factory.GetType()}");
    }

    public override TOutput Create(T1 input1, T2 input2) {
      try {
        return _factory.Create(input1, input2);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(ErrorableFactoryTryCatchDecorator<object, object, object>)}\r\n" +
                         $"{LogExtraInformation(input1, input2)}", e);
        return _factory.OnErrorCreate;
      }
    }

    public TOutput OnErrorCreate => _factory.OnErrorCreate;
  }

  public class ErrorableFactoryTryCatchDecorator<T1, T2, T3, TOutput> : FactoryTryCatchDecorator<T1, T2, T3, TOutput>, IErrorableFactory<T1, T2, T3, TOutput> {
    private readonly IErrorableFactory<T1, T2, T3, TOutput> _factory;
    private readonly ILogger _logger;

    public ErrorableFactoryTryCatchDecorator(IErrorableFactory<T1, T2, T3, TOutput> factory, ILogger logger) : base(factory, logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(ErrorableFactoryTryCatchDecorator<object, object, object, object>)} now decorates {_factory.GetType()}");
    }

    public override TOutput Create(T1 input1, T2 input2, T3 input3) {
      try {
        return _factory.Create(input1, input2, input3);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(ErrorableFactoryTryCatchDecorator<object, object, object, object>)}\r\n" +
                         $"{LogExtraInformation(input1, input2, input3)}", e);
        return _factory.OnErrorCreate;
      }
    }

    public TOutput OnErrorCreate => _factory.OnErrorCreate;
  }

}
