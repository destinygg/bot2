using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class DelegatedFactory<T, TResult> : IFactory<T, TResult> {
    private readonly Func<T, TResult> _create;

    public DelegatedFactory(Func<T, TResult> create) {
      _create = create;
    }

    public TResult Create(T input) => _create(input);
  }
}
