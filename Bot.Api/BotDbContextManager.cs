using System;
using Microsoft.EntityFrameworkCore;

namespace Bot.Api {
  public class BotDbContextManager {

    public void CallWithForeignKeysAndSaving(Action<BotDbContext> injectedAction) {
      using (var context = new BotDbContext()) {
        context.Database.ExecuteSqlCommand("PRAGMA foreign_keys = ON");
        injectedAction(context);
        context.SaveChanges();
      }
    }

  }
}
