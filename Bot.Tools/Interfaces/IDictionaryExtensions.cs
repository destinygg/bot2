using System;
using System.Collections.Generic;

namespace Bot.Tools {
  public static class IDictionaryExtensions {

    /// <summary>
    /// Ensures a key/value pair exists within a dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of key used in <paramref name="dictionary"/>.</typeparam>
    /// <typeparam name="TValue">The type of value used in <paramref name="dictionary"/>.</typeparam>
    /// <param name="dictionary">The dictionary to ensure the key/value pair is added.</param>
    /// <param name="key">The key.</param>
    /// <param name="getValue">A function that returns a value for the provided key. Will not be called if <paramref name="key"/> is already in <paramref name="dictionary"/>.</param>
    public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> getValue) {
      if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
      if (getValue == null) throw new ArgumentNullException(nameof(getValue));
      if (!dictionary.ContainsKey(key)) {
        dictionary[key] = getValue();
      }
    }

    /// <summary>
    /// Ensures a key/value pair exists within a dictionary, and then returns the value.
    /// </summary>
    /// <typeparam name="TKey">The type of key used in <paramref name="dictionary"/>.</typeparam>
    /// <typeparam name="TValue">The type of value used in <paramref name="dictionary"/>.</typeparam>
    /// <param name="dictionary">The dictionary to ensure the key/value pair is added.</param>
    /// <param name="key">The key.</param>
    /// <param name="getValue">A function that returns a value for the provided key. Will not be called if <paramref name="key"/> is already in <paramref name="dictionary"/>.</param>
    /// <returns>The value already in the dictionary for <paramref name="key"/> if present, otherwise the result of calling <paramref name="getValue"/>.</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> getValue) {
      if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
      if (getValue == null) throw new ArgumentNullException(nameof(getValue));
      // TryGetValue is ContainsKey plus an array lookup if it was found,
      // so we save a lookup by not writing this method in terms of the other method above.
      TValue value;
      return dictionary.TryGetValue(key, out value) ? value : (dictionary[key] = getValue());
    }

  }
}
