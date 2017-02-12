using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class DelegatedProvider<TResult> : IProvider<TResult> {
    private readonly Func<TResult> _get;

    public DelegatedProvider(Func<TResult> get) {
      _get = get;
    }

    public TResult Get() =>
      _get();
  }
}
