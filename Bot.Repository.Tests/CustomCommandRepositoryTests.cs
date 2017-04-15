using System.Linq;
using Bot.Repository.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class CustomCommandRepositoryTests {

    [TestMethod]
    public void CustomCommandsRepository_GetAll_ReturnsMasterData() {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var repository = container.GetInstance<IQueryCommandService<IUnitOfWork>>();

      var customCommands = repository.Query(r => r.CustomCommand.GetAll);

      Assert.IsTrue(customCommands.Single(c => c.Command == "rules").Response == "github.com/destinygg/bot2");
    }

    [TestMethod]
    public void CustomCommandsRepository_AddOnce_PersistsCustomData() {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var command = TestHelper.RandomString();
      var response = TestHelper.RandomString();
      var repository = container.GetInstance<IQueryCommandService<IUnitOfWork>>();

      repository.Command(r => r.CustomCommand.Add(command, response));

      var customCommands = repository.Query(r => r.CustomCommand.GetAll);
      Assert.IsTrue(customCommands.Single(c => c.Command == command).Response == response);
    }

    [TestMethod]
    public void CustomCommandsRepository_AddTwice_ThrowsConstraintFailedException() {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var command = TestHelper.RandomString();
      var response = TestHelper.RandomString();
      var repository = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      repository.Command(r => r.CustomCommand.Add(command, response));

      var exception = TestHelper.AssertCatch<DbUpdateException>(() => repository.Command(r => r.CustomCommand.Add(command, response)));

      Assert.AreEqual("SQLite Error 19: 'constraint failed'.", exception.InnerException.Message);
    }

  }
}
