using System;
using Bot.Database.Interfaces;
using Bot.Tools;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database {
  public class BotDbContextManager {
    private readonly IDatabaseService<IBotDbContext> _databaseService;

    public BotDbContextManager(IDatabaseService<IBotDbContext> databaseService) {
      _databaseService = databaseService;
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
