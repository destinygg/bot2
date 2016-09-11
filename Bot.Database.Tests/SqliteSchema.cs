using Bot.Database.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;

namespace Bot.Database.Tests {
  [TestClass]
  public class SqliteSchema {
    [TestMethod]
    public void JsonUserHistorySchema() {
      var db = new SQLiteConnection("Bot.sqlite");
      var tableInfo = db.GetTableInfo(nameof(JsonUserHistory));
      var actual = tableInfo.Exists(x => x.Name == "Id");
      Assert.IsTrue(actual);
    }
  }
}
