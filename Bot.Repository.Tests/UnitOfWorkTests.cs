using Bot.Database.Entities;
using Bot.Repository.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class UnitOfWorkTests {

    [TestMethod]
    public void UpdatePunishedUser() {
      var unitOfWorkService = RepositoryHelper.GetContainerWithInitializedAndIsolatedRepository().GetInstance<IQueryCommandService<IUnitOfWork>>();
      var nick = TestHelper.RandomString();
      var oldCount = TestHelper.RandomInt();
      var punishedUserWrite = new PunishedUser {
        User = new User { Nick = nick },
        AutoPunishment = new AutoPunishment(),
        Count = oldCount,
      };
      unitOfWorkService.Command(u => u.PunishedUsers.Add(punishedUserWrite));
      var userToUpdate = unitOfWorkService.Query(u => u.PunishedUsers.SingleOrDefault(x => x.User.Nick == nick));
      var newCount = oldCount + 1;
      userToUpdate.Count = newCount;

      unitOfWorkService.Command(u => u.PunishedUsers.Update(userToUpdate));

      var updatedUser = unitOfWorkService.Query(u => u.PunishedUsers.SingleOrDefault(x => x.User.Nick == nick));
      Assert.AreEqual(updatedUser.Count, newCount);
    }

  }
}
