using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  public abstract class BaseRepositoryTests {
    private static Random _random;

    [TestInitialize]
    public void Initialize() {
      var manager = new DatabaseManager();
      manager.EnsureDeleted();
      manager.EnsureCreated();
      var seed = Guid.NewGuid().GetHashCode();
      _random = new Random(seed);
      Trace.WriteLine($"Seed is: {seed}");
    }

    [TestCleanup]
    public void Cleanup() {
      var manager = new DatabaseManager();
      manager.EnsureDeleted();
    }

    protected static string RandomString(int length = 10) {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
      return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    protected static int RandomInt() => _random.Next();
  }
}
