using Bot.Database.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;

namespace Bot.Database.Tests {
  [TestClass]
  public class StateVariablesSchema {
    private const string DbFileName = "Bot.sqlite";

    [TestMethod]
    public void StateVariablesSchemaKey() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(StateVariables));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(StateVariables.Key));

        // Assert
        Assert.IsTrue(actual);
      }
    }

    [TestMethod]
    public void StateVariablesSchemaValue() {
      // Arrange
      using (var db = new SQLiteConnection(DbFileName)) {
        var tableInfo = db.GetTableInfo(nameof(StateVariables));

        // Act
        var actual = tableInfo.Exists(x => x.Name == nameof(StateVariables.Value));

        // Assert
        Assert.IsTrue(actual);
      }
    }
  }
}
