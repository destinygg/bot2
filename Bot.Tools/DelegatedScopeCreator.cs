using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class DelegatedScopeCreator : IScopeCreator {

    private readonly Func<IDisposable> _createScope;

    public DelegatedScopeCreator(Func<IDisposable> createScope) {
      _createScope = createScope;
    }

    IDisposable IScopeCreator.CreateScope() => _createScope();
  }
}
