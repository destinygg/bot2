using System;
using System.Linq;

namespace Bot.Database.Interfaces {
  public static class IDatabaseServiceExtensions {
    /// <summary>
    /// Executes the given commands in sequence and returns total number of objects written to the underlying database.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="dbService">The instance of the interface on which this extension method is defined.</param>
    /// <param name="commands">The sequence of commands of execute.</param>
    /// <returns>The total number of objects written to the underlying database from all of the given <paramref name="commands"/>.</returns>
    public static int Command<TContext>(this IDatabaseService<TContext> dbService, params Action<TContext>[] commands)
      where TContext : IDisposable, ISavable =>
        dbService.Command(db => commands
          .Where(cmd => cmd != null)
          .Sum(cmd => {
            cmd(db);
            return db.SaveChanges();
          }));
  }
}
