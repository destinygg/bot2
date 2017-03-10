using System.Collections.Generic;
using System.Linq;
using Bot.Database;
using Bot.Database.Entities;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class UserRepositoryTests {

    [TestMethod]
    public void ReadWriteUser() {
      var container = RepositoryHelper.GetContainerWithInitializedAndIsolatedRepository();

      var userName = TestHelper.RandomString();
      using (var context = container.GetInstance<BotDbContext>()) {
        var userRepository = new UserRepository(context.Users);
        userRepository.Add(new User { Nick = userName });
        context.SaveChanges();
      }

      IEnumerable<User> testRead;
      using (var context = container.GetInstance<BotDbContext>()) {
        var userRepository = new UserRepository(context.Users);
        testRead = userRepository.GetAll();
      }

      Assert.AreEqual(userName, testRead.Single().Nick);
    }

  }
}
