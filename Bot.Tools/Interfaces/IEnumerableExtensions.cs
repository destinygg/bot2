using System;
using System.Collections.Generic;
using System.Linq;

namespace Bot.Tools.Interfaces {
  public static class IEnumerableExtensions {
    public static TData ArgMax<TData, TColumn>(this IEnumerable<TData> set, Func<TData, TColumn> compareBy) =>
      set.OrderByDescending(compareBy).FirstOrDefault();
  }
}
