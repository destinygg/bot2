using Bot.Database.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;

namespace Bot.Database.Tests {
  [TestClass]
  public class JsonUserHistorySchema {
    private const string DbFileName = "Bot.sqlite";

    [TestMethod]
    public void JsonUserHistorySchemaId() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(JsonUserHistory));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(JsonUserHistory.Id));

        // Assert
        Assert.IsTrue(actual);
      }
    }

    [TestMethod]
    public void JsonUserHistorySchemaNick() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(JsonUserHistory));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(JsonUserHistory.Nick));

        // Assert
        Assert.IsTrue(actual);
      }
    }

    [TestMethod]
    public void JsonUserHistorySchemaRawHistory() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(JsonUserHistory));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(JsonUserHistory.RawHistory));

        // Assert
        Assert.IsTrue(actual);
      }
    }
  }
}
