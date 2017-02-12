using System;
using System.Diagnostics;
using System.Linq;
using Bot.Database.Entities;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  public abstract class BaseRepositoryTests {
    private Random _random;
    private readonly DatabaseInitializer _databaseInitializer;

    protected BaseRepositoryTests() {
      var containerManager = new ContainerManager();
      _databaseInitializer = containerManager.Container.GetInstance<DatabaseInitializer>();
    }

    [TestInitialize]
    public void Initialize() {
      _databaseInitializer.EnsureDeleted();
      _databaseInitializer.EnsureCreated();
      _databaseInitializer.AddMasterData();
      var seed = Guid.NewGuid().GetHashCode();
      _random = new Random(seed);
      Trace.WriteLine($"Seed is: {seed}");
    }

    [TestCleanup]
    public void Cleanup() {
      _databaseInitializer.EnsureCreated();
    }

    protected string RandomString(int length = 10) {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
      return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    protected int RandomInt() => _random.Next();

    protected AutoPunishmentType RandomAutoPunishmentType() {
      var values = Enum.GetValues(typeof(AutoPunishmentType));
      return (AutoPunishmentType) values.GetValue(_random.Next(values.Length));
    }

  }
}
