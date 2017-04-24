using System;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Tools {

  public class GenericClassFactoryTryCatchDecorator<T1, T2, T3> : IGenericClassFactory<T1, T2, T3> {
    private readonly IGenericClassFactory<T1, T2, T3> _factory;
    private readonly ILogger _logger;

    public GenericClassFactoryTryCatchDecorator(IGenericClassFactory<T1, T2, T3> factory, ILogger logger) {
      _factory = factory;
      _logger = logger;
      _logger.LogInformation($"{nameof(GenericClassFactoryTryCatchDecorator<object, object, object>)} now decorates {_factory.GetType()}");
    }

    public TResult Create<TResult>(T1 input1, T2 input2, T3 input3)
      where TResult : class {
      try {
        return _factory.Create<TResult>(input1, input2, input3);
      } catch (Exception e) {
        _logger.LogError($"Error occured in {nameof(GenericClassFactoryTryCatchDecorator<object, object, object>)}", e);
        _logger.LogError(LogExtraInformation(input1, input2, input3));
        return null;
      }
    }

    private string LogExtraInformation(T1 input1, T2 input2, T3 input3) {
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
