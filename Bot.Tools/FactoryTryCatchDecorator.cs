using System;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Tools {
  public class FactoryTryCatchDecorator<TOutput> : IFactory<TOutput> {
    private readonly IFactory<TOutput> _factory;
    private readonly ILogger _logger;

    public FactoryTryCatchDecorator(IFactory<TOutput> factory, ILogger logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(FactoryTryCatchDecorator<object, object>)} now decorates {_factory.GetType()}");
    }

    public virtual TOutput Create() {
      try {
        return _factory.Create();
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(FactoryTryCatchDecorator<object, object>)}", e);
        _logger.LogError(LogExtraInformation());
        throw;
      }
    }

    protected string LogExtraInformation() {
      try {
        return
          $"{nameof(_factory)} is of type {_factory.GetType()}";
      } catch (Exception e) {
        return $"Error logging extra information: {e}";
      }
    }
  }

  public class FactoryTryCatchDecorator<T, TOutput> : IFactory<T, TOutput> {
    private readonly IFactory<T, TOutput> _factory;
    private readonly ILogger _logger;

    public FactoryTryCatchDecorator(IFactory<T, TOutput> factory, ILogger logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(FactoryTryCatchDecorator<object, object>)} now decorates {_factory.GetType()}");
    }

    public virtual TOutput Create(T input) {
      try {
        return _factory.Create(input);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(FactoryTryCatchDecorator<object, object>)}", e);
        _logger.LogError(LogExtraInformation(input));
        throw;
      }
    }

    protected string LogExtraInformation(T input) {
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
  }

  public class FactoryTryCatchDecorator<T1, T2, TOutput> : IFactory<T1, T2, TOutput> {
    private readonly IFactory<T1, T2, TOutput> _factory;
    private readonly ILogger _logger;

    public FactoryTryCatchDecorator(IFactory<T1, T2, TOutput> factory, ILogger logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(FactoryTryCatchDecorator<object, object, object>)} now decorates {_factory.GetType()}");
    }

    public virtual TOutput Create(T1 input1, T2 input2) {
      try {
        return _factory.Create(input1, input2);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(FactoryTryCatchDecorator<object, object, object>)}", e);
        _logger.LogError(LogExtraInformation(input1, input2));
        throw;
      }
    }

    protected string LogExtraInformation(T1 input1, T2 input2) {
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
  }

  public class FactoryTryCatchDecorator<T1, T2, T3, TOutput> : IFactory<T1, T2, T3, TOutput> {
    private readonly IFactory<T1, T2, T3, TOutput> _factory;
    private readonly ILogger _logger;

    public FactoryTryCatchDecorator(IFactory<T1, T2, T3, TOutput> factory, ILogger logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(FactoryTryCatchDecorator<object, object, object, object>)} now decorates {_factory.GetType()}");
    }

    public virtual TOutput Create(T1 input1, T2 input2, T3 input3) {
      try {
        return _factory.Create(input1, input2, input3);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(FactoryTryCatchDecorator<object, object, object, object>)}", e);
        _logger.LogError(LogExtraInformation(input1, input2, input3));
        throw;
      }
    }

    protected string LogExtraInformation(T1 input1, T2 input2, T3 input3) {
      try {
        return
          $"{nameof(_factory)} is of type {_factory.GetType()}\r\n" +
          $"{nameof(T1)} is {typeof(T1)}\r\n" +
          $"{nameof(input1)} is of type {input1.GetType()}\r\n" +
          $"{nameof(input1)} is {ObjectDumper.Dump(input1, 10)}\r\n" +
          $"{nameof(T2)} is {typeof(T2)}\r\n" +
          $"{nameof(input2)} is of type {input2.GetType()}\r\n" +
          $"{nameof(input2)} is {ObjectDumper.Dump(input2, 10)}\r\n" +
          $"{nameof(T3)} is {typeof(T3)}\r\n" +
          $"{nameof(input3)} is of type {input3.GetType()}\r\n" +
          $"{nameof(input3)} is {ObjectDumper.Dump(input3, 10)}";
      } catch (Exception e) {
        return $"Error logging extra information: {e}";
      }
    }
  }

}
