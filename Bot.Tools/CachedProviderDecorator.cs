using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class CachedProviderDecorator<TOut> : IProvider<TOut> {

    private readonly Lazy<TOut> _cachedValue;

    public CachedProviderDecorator(IProvider<TOut> decorated) {
      _cachedValue = new Lazy<TOut>(decorated.Get);
    }

    TOut IProvider<TOut>.Get() => _cachedValue.Value;
  }
}
