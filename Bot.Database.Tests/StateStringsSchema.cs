using Bot.Database.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;

namespace Bot.Database.Tests {
  [TestClass]
  public class StateStringsSchema {
    private const string DbFileName = "Bot.sqlite";

    [TestMethod]
    public void StateStringsSchemaKey() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(StateStrings));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(StateStrings.Key));

        // Assert
        Assert.IsTrue(actual);
      }
    }

    [TestMethod]
    public void StateStringsSchemaValue() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(StateStrings));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(StateStrings.Value));

        // Assert
        Assert.IsTrue(actual);
      }
    }
  }
}
