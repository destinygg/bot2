using Bot.Database.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;

namespace Bot.Database.Tests {
  [TestClass]
  public class StalkSchema {
    private const string DbFileName = "Bot.sqlite";

    [TestMethod]
    public void StalkSchemaId() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(Stalk));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(Stalk.Id));

        // Assert
        Assert.IsTrue(actual);
      }
    }

    [TestMethod]
    public void StalkHistorySchemaNick() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(Stalk));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(Stalk.Nick));

        // Assert
        Assert.IsTrue(actual);
      }
    }

    [TestMethod]
    public void StalkSchemaRawTime() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(Stalk));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(Stalk.Time));

        // Assert
        Assert.IsTrue(actual);
      }
    }

    [TestMethod]
    public void StalkSchemaRawText() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(Stalk));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(Stalk.Text));

        // Assert
        Assert.IsTrue(actual);
      }
    }
  }
}
