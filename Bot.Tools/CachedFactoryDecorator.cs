using System.Collections.Generic;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class CachedFactoryDecorator<TIn, TOut> : IFactory<TIn, TOut> {

    private readonly IFactory<TIn, TOut> _decorated;
    private readonly Dictionary<TIn, TOut> _cachedValues = new Dictionary<TIn, TOut>();

    public CachedFactoryDecorator(IFactory<TIn, TOut> decorated) {
      _decorated = decorated;
    }

    TOut IFactory<TIn, TOut>.Create(TIn input) => _cachedValues.GetOrAdd(input, () => _decorated.Create(input));

  }
}
