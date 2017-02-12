using System;
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

    /// <summary>
    /// Applies a function to a value. Useful for method chaining in fluent syntax scenarios.
    /// </summary>
    /// <typeparam name="TInput">The value to apply to the <paramref name="function"/>.</typeparam>
    /// <param name="source">The value to be applied.</param>
    /// <param name="function">A function to be applied to the <paramref name="source"/>.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>; the result of applying <paramref name="function"/> to <paramref name="source"/>.</returns>
    public static TResult Apply<TInput, TResult>(this TInput source, Func<TInput, TResult> function) => function(source);

    /// <summary>
    /// Applies an action to a value. Useful for method chaining in fluent syntax scenarios.
    /// </summary>
    /// <typeparam name="TInput">The value to apply to the <paramref name="action"/>.</typeparam>
    /// <param name="source">The value to be applied.</param>
    /// <param name="action">An action to be applied to the <paramref name="source"/>.</param>
    public static void Apply<TInput>(this TInput source, Action<TInput> action) => action(source);
  }
}
