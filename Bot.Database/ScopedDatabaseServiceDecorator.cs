using System;
using Bot.Database.Interfaces;

namespace Bot.Database {
  /// <summary>
  /// Defines the execution scope to be the duration of the query/command.
  /// </summary>
  public class ScopedDatabaseServiceDecorator<TContext> : IDatabaseService<TContext>
    where TContext : IDisposable, ISavable {

    private readonly IDatabaseService<TContext> _decoratedDatabaseService;
    private readonly IScopeCreator _lifetimeScoper;

    public ScopedDatabaseServiceDecorator(IDatabaseService<TContext> decoratedDatabaseService, IScopeCreator lifetimeScoper) {
      this._decoratedDatabaseService = decoratedDatabaseService;
      this._lifetimeScoper = lifetimeScoper;
    }

    TResult IDatabaseService<TContext>.Query<TResult>(Func<TContext, TResult> query) => _Execute(() => _decoratedDatabaseService.Query(query));
    int IDatabaseService<TContext>.Command(Func<TContext, int> command) => _Execute(() => _decoratedDatabaseService.Command(command));

    private T _Execute<T>(Func<T> body) {
      using (_lifetimeScoper.CreateScope()) {
        return body();
      }
    }

  }
}
