using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  /// <summary>
  /// Defines the execution scope to be the duration of the query/command.
  /// </summary>
  public class ScopedQueryCommandServiceDecorator<TContext> : IQueryCommandService<TContext>
    where TContext : IDisposable, ISavable {

    private readonly IQueryCommandService<TContext> _decoratedQueryCommandService;
    private readonly IScopeCreator _lifetimeScoper;

    public ScopedQueryCommandServiceDecorator(IQueryCommandService<TContext> decoratedQueryCommandService, IScopeCreator lifetimeScoper) {
      this._decoratedQueryCommandService = decoratedQueryCommandService;
      this._lifetimeScoper = lifetimeScoper;
    }

    TResult IQueryCommandService<TContext>.Query<TResult>(Func<TContext, TResult> query) => _Execute(() => _decoratedQueryCommandService.Query(query));
    int IQueryCommandService<TContext>.Command(Func<TContext, int> command) => _Execute(() => _decoratedQueryCommandService.Command(command));

    private T _Execute<T>(Func<T> body) {
      using (_lifetimeScoper.CreateScope()) {
        return body();
      }
    }

  }
}
