using System;
using System.Collections.Generic;
using System.Linq;

namespace Bot.Tools {
  public static class MergeExtensions {

    public static void Merge<TDest, TSource>(this IEnumerable<TDest> destination,
        IEnumerable<TSource> source,
        Func<TDest, TSource, bool> predicate,
        Func<TSource, TDest> create,
        Action<TDest> delete,
        Action<TDest> add,
        Action<TDest, TSource> update
    ) {
      //List < (TDest d, TSource s)> updates = null; // todo Update to VS2017 and use the new tuple syntax
      List<Tuple<TDest, TSource>> updates = null;
      List<TSource> adds = null;
      List<TDest> deletes = null;

      source = source ?? Enumerable.Empty<TSource>();
      if (update != null)
        updates = (
            from d in destination
            from s in source
            where predicate(d, s)
            //select (d, s)
            select Tuple.Create(d, s)
        ).ToList();
      if (add != null && create != null)
        adds = source.Where(s => !destination.Any(d => predicate(d, s))).ToList();
      if (delete != null) deletes = destination.Where(d => !source.Any(s => predicate(d, s))).ToList();

      //updates?.ForEach(t => update(t.d, t.s));
      updates?.ForEach(t => update(t.Item1, t.Item2));
      deletes?.ForEach(delete);
      adds?.ForEach(s => {
        TDest d = create(s);
        update?.Invoke(d, s);
        add.Invoke(d);
      });
    }

  }
}
