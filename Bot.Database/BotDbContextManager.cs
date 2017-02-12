using System;
using Bot.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class BotDbContextManager {

    public void Save(Action<IBotDbContext> dbCommand) {
      using (var context = new BotDbContext()) {
        _InvokeWithEnforcedForeignKeys(dbCommand, context);
        context.SaveChanges();
      }
    }

    private void _InvokeWithEnforcedForeignKeys(Action<IBotDbContext> dbCommand, IBotDbContext context) {
      context.Database.ExecuteSqlCommand("PRAGMA foreign_keys = ON");
      dbCommand(context);
    }

  }
}
