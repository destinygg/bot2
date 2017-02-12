using System;
using Bot.Database.Interfaces;
using Bot.Tools;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class BotDbContextManager {
    private readonly DatabaseService<IBotDbContext> _databaseService;

    public BotDbContextManager() {
      var delegatedProvider = new DelegatedProvider<BotDbContext>(() => new BotDbContext());
      _databaseService = new DatabaseService<IBotDbContext>(delegatedProvider);
    }

    public void Save(Action<IBotDbContext> dbCommand) {
      _databaseService.Command(db => {
        _InvokeWithEnforcedForeignKeys(dbCommand, db);
        db.SaveChanges();
      });
    }

    private void _InvokeWithEnforcedForeignKeys(Action<IBotDbContext> dbCommand, IBotDbContext context) {
      context.Database.ExecuteSqlCommand("PRAGMA foreign_keys = ON");
      dbCommand(context);
    }

  }
}
