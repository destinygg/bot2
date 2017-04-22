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
        _logger.LogError(LogExtraInformation(input));
        return _factory.OnErrorCreate;
      }
    }

    private string LogExtraInformation(T input) {
      try {
        return
          $"{nameof(_factory)} is of type {_factory.GetType()}\r\n" +
          $"{nameof(T)} is {typeof(T)}\r\n" +
          $"{nameof(input)} is of type {input.GetType()}\r\n" +
          $"{nameof(input)} is {ObjectDumper.Dump(input, 10)}";
      } catch (Exception e) {
        return $"Error logging extra information: {e}";
      }
    }

    public TOutput OnErrorCreate => _factory.OnErrorCreate;
  }

  public class FactoryTryCatchDecorator<T1, T2, TOutput> : IErrorableFactory<T1, T2, TOutput> {
    private readonly IErrorableFactory<T1, T2, TOutput> _factory;
    private readonly ILogger _logger;

    public FactoryTryCatchDecorator(IErrorableFactory<T1, T2, TOutput> factory, ILogger logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(FactoryTryCatchDecorator<object, object, object>)} now decorates {_factory.GetType()}");
    }

    public TOutput Create(T1 input1, T2 input2) {
      try {
        return _factory.Create(input1, input2);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(FactoryTryCatchDecorator<object, object, object>)}", e);
        _logger.LogError(LogExtraInformation(input1, input2));
        return _factory.OnErrorCreate;
      }
    }

    private string LogExtraInformation(T1 input1, T2 input2) {
      try {
        return
          $"{nameof(_factory)} is of type {_factory.GetType()}\r\n" +
          $"{nameof(T1)} is {typeof(T1)}\r\n" +
          $"{nameof(input1)} is of type {input1.GetType()}\r\n" +
          $"{nameof(input1)} is {ObjectDumper.Dump(input1, 10)}\r\n" +
          $"{nameof(T2)} is {typeof(T2)}\r\n" +
          $"{nameof(input2)} is of type {input2.GetType()}\r\n" +
          $"{nameof(input2)} is {ObjectDumper.Dump(input2, 10)}";
      } catch (Exception e) {
        return $"Error logging extra information: {e}";
      }
    }

    public TOutput OnErrorCreate => _factory.OnErrorCreate;
  }
}
