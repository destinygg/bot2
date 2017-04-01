using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Tools.Tests {
  [TestClass]
  public class MergeTests {
    [TestMethod]
    public void SimpleMerge() {
      var a = new List<Person> { new Person(1, "a"), new Person(2, "c") };
      var b = new List<Person> { new Person(1, "b"), new Person(9, "b") };

      b.Merge(
          source: a,
          predicate: (person1, person2) => person1.Id == person2.Id,
          create: person => person,
          delete: person => b.Remove(person),
          add: person => b.Add(person),
          update: (d, s) => {
            d.Name = s.Name;
          });

      Assert.IsTrue(a.Select(x => x.Id).SequenceEqual(b.Select(x => x.Id)));
      Assert.IsTrue(a.Select(x => x.Name).SequenceEqual(b.Select(x => x.Name)));
    }

    private class Person {
      public Person(int id, string name) {
        Id = id;
        Name = name;
      }

      public int Id { get; }
      public string Name { get; set; }
      public override string ToString() => Id + "." + Name;
    }

  }
}
