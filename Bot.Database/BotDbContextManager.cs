using System;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class BotDbContextManager {

    public void Save(Action<BotDbContext> dbCommand) {
      using (var context = new BotDbContext()) {
        _InvokeWithEnforcedForeignKeys(dbCommand, context);
        context.SaveChanges();
      }
    }

    private void _InvokeWithEnforcedForeignKeys(Action<BotDbContext> dbCommand, BotDbContext context) {
      context.Database.ExecuteSqlCommand("PRAGMA foreign_keys = ON");
      dbCommand(context);
    }

  }
}
