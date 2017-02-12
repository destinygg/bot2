using System;
using System.Diagnostics;
using System.Linq;
using Bot.Database.Entities;

namespace Bot.Database.Tests {
  internal static class TestHelper {

    public static int RandomInt() => Random().Next();

    public static AutoPunishmentType RandomAutoPunishmentType() {
      var values = Enum.GetValues(typeof(AutoPunishmentType));
      return (AutoPunishmentType) values.GetValue(Random().Next(values.Length));
    }

    public static Random Random() {
      var seed = Guid.NewGuid().GetHashCode();
      Trace.WriteLine($"Seed is: {seed}");
      return new Random(seed);
    }

    public static string RandomString(int length = 10) {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
      return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[Random().Next(s.Length)]).ToArray());
    }

  }
}
