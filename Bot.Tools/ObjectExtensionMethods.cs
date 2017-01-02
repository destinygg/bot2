using System.Collections.Generic;

namespace Bot.Tools {
  public static class ObjectExtensionMethods {
    /// <summary>
    /// Converts a single item into an IEnumerable of 1 item.
    /// </summary>
    /// <typeparam name="T">The type of the object being passed in.</typeparam>
    /// <param name="source">The object to convert to an IEnumerable containing this item</param>
    /// <returns>An IEnumerable containing the passed in item</returns>
    public static IEnumerable<T> Wrap<T>(this T source) {
      yield return source;
    }

    public static IList<T> WrapWithList<T>(this T source) => new List<T> { source };
  }
}
