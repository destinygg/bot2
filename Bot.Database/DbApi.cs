using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Tools;
using SQLite;

namespace Bot.Database {
  public class DbApi {
    protected SQLiteConnection Db => new SQLiteConnection(Settings.SqlitePath);
  }
}
