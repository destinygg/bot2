using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Database.Tests {
  [TestClass]
  public class UserRepositoryTests : BaseRepositoryTests {

    [TestMethod]
    public void ReadWriteUser() {
      // Arrange
      var userName = "User1";

      // Act
      using (var context = new BotDbContext()) {
        var userRepository = new UserRepository(context.Users);
        userRepository.Add(new User { Nick = userName });
        context.SaveChanges();
      }

      IEnumerable<User> testRead;
      using (var context = new BotDbContext()) {
        var userRepository = new UserRepository(context.Users);
        testRead = userRepository.GetAll();
      }

      // Assert
      Assert.AreEqual(userName, testRead.Single().Nick);
    }

  }
}
