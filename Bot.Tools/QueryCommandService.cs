using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class QueryCommandService<TContext> : IQueryCommandService<TContext>
    where TContext : IDisposable, ISavable {

    private readonly IProvider<TContext> _contextProvider;

    public QueryCommandService(IProvider<TContext> contextProvider) {
      _contextProvider = contextProvider;
    }

    TResult IQueryCommandService<TContext>.Query<TResult>(Func<TContext, TResult> query) =>
      _execute(query);

    int IQueryCommandService<TContext>.Command(Func<TContext, int> command) =>
      _execute(command);

    private TResult _execute<TResult>(Func<TContext, TResult> body) =>
      _contextProvider.Get().Apply(body);

  }
}
