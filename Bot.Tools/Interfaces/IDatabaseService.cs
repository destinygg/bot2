using System;

namespace Bot.Tools.Interfaces {
  /// <summary>
  /// Provides access to the database <typeparamref name="TContext"/>
  /// through a read method called <see cref="Query{TResult}(Func{TContext,TResult})"/>
  /// and a write method called <see cref="Command(Func{TContext, int})"/>.
  /// </summary>
  /// <typeparam name="TContext">The type of database context.</typeparam>
  public interface IDatabaseService<out TContext>
    where TContext : IDisposable, ISavable {
    /// <summary>
    /// Executes the given query and returns its result.
    /// </summary>
    /// <typeparam name="TResult">Type of the data returned by the query.</typeparam>
    /// <param name="query">The query to execute, which has no side effects.</param>
    /// <returns>The result of the given query.</returns>
    TResult Query<TResult>(Func<TContext, TResult> query);

    /// <summary>
    /// Executes the given command.
    /// The command saves its changes and returns the number of objects written to the underlying database.
    /// </summary>
    /// <remarks>
    /// Instead of calling this method directly,
    /// it is easier (and recommended) to call the extenion method <seealso cref="IDatabaseServiceExtensions.Command{TContext}(IDatabaseService{TContext}, Action{TContext}[])"/>.
    /// </remarks>
    /// <param name="command">The command to execute, which saves changes and returns the number of objects written to the underlying database.</param>
    /// <returns>The number of objects written to the underlying database.</returns>
    int Command(Func<TContext, int> command);
  }
}
