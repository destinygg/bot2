using System;
using System.Diagnostics;
using System.Linq;
using Bot.Database.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  public static class TestHelper {

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

    public static TException AssertCatch<TException>(Action action)
      where TException : Exception {
      try {
        action();
        throw new AssertFailedException($"Expected exception of type {typeof(TException)} was not thrown");
      } catch (TException exception) {
        return exception;
      }
    }
  }
}
