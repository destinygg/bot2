﻿using Bot.Database.Entities;
using Bot.Models;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Repository.Tests {
  [TestClass]
  public class UnitOfWorkTests {

    //[TestMethod]
    //public void UpdatePunishedUser() {
    //  var unitOfWorkService = RepositoryHelper.GetContainerWithInitializedAndIsolatedRepository().GetInstance<IQueryCommandService<IUnitOfWork>>();
    //  var nick = TestHelper.RandomString();
    //  var oldCount = TestHelper.RandomInt();
    //  var punishedUserWrite = new PunishedUser {
    //    Nick = nick,
    //    AutoPunishment = new AutoPunishment(),
    //    Count = oldCount,
    //  };
    //  unitOfWorkService.Command(u => u.PunishedUsers.Add(punishedUserWrite));
    //  var userToUpdate = unitOfWorkService.Query(u => u.PunishedUsers.SingleOrDefault(x => x.Nick == nick));
    //  var newCount = oldCount + 1;
    //  userToUpdate.Count = newCount;

    //  unitOfWorkService.Command(u => u.PunishedUsers.Update(userToUpdate));

    //  var updatedUser = unitOfWorkService.Query(u => u.PunishedUsers.SingleOrDefault(x => x.Nick == nick));
    //  Assert.AreEqual(updatedUser.Count, newCount);
    //}

  }
}
