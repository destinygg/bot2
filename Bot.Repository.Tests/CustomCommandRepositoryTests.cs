using System;
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

      Assert.IsTrue(exception.InnerException.Message.Contains("SQLite Error 19"));
    }

    [TestMethod]
    public void CustomCommandsRepository_RemoveMasterData_RemovesMasterData() {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var repository = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var masterDataCommand = "rules";

      var linesChanged = repository.Command(r => r.CustomCommand.Delete(masterDataCommand));

      var customCommands = repository.Query(r => r.CustomCommand.GetAll);
      Assert.AreEqual(0, customCommands.Count(c => c.Command == masterDataCommand));
      Assert.AreEqual(1, linesChanged);
    }

    [TestMethod]
    public void CustomCommandsRepository_RemoveNonexistantCommand_ThrowsException() {
      var container = new TestContainerManager().InitializeAndIsolateRepository();
      var repository = container.GetInstance<IQueryCommandService<IUnitOfWork>>();
      var nonexistantCommand = TestHelper.RandomString();
      var customCommands = repository.Query(r => r.CustomCommand.GetAll);
      Assert.IsNull(customCommands.SingleOrDefault(c => c.Command == nonexistantCommand));

      var exception = TestHelper.AssertCatch<InvalidOperationException>(() => repository.Command(r => r.CustomCommand.Delete(nonexistantCommand)));

      Assert.AreEqual("Sequence contains no elements", exception.Message);
      customCommands = repository.Query(r => r.CustomCommand.GetAll);
      Assert.IsNull(customCommands.SingleOrDefault(c => c.Command == nonexistantCommand));
    }

  }
}
