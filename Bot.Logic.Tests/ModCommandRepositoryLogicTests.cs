using System;
using System.Linq;
using Bot.Database.Entities;
using Bot.Logic.Interfaces;
using Bot.Main.Moderate;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandRepositoryLogicTests {

    private static void AddMute(IModCommandRepositoryLogic modCommandLogic, string mutedPhrase, TimeSpan firstDuration, IQueryCommandService<IUnitOfWork> unitOfWork) {
      modCommandLogic.AddMute(mutedPhrase, firstDuration);
      var autoPunishments = unitOfWork.Query(u => u.AutoPunishments.GetAllWithUser);
      var autoPunishment = autoPunishments.Single();
      Assert.AreEqual(mutedPhrase, autoPunishment.Term);
      Assert.AreEqual(AutoPunishmentType.MutedString, autoPunishment.Type);
      Assert.AreEqual(firstDuration, autoPunishment.Duration);
    }

    [TestMethod]
    public void ModCommandRepositoryLogic_AddMuteEmptyDb_AddsToDb() {
      var expectedMute = "words are wind";
      var expectedDuration = TimeSpan.FromSeconds(1);
      var expectedType = AutoPunishmentType.MutedString;
      var testContainerManager = new TestContainerManager().InitializeAndIsolateRepository();
      var modCommandLogic = testContainerManager.GetInstance<IModCommandRepositoryLogic>();
      var unitOfWork = testContainerManager.GetInstance<IQueryCommandService<IUnitOfWork>>();

      modCommandLogic.AddMute(expectedMute, expectedDuration);

      var autoPunishments = unitOfWork.Query(u => u.AutoPunishments.GetAllWithUser);
      var autoPunishment = autoPunishments.Single();
      Assert.AreEqual(expectedMute, autoPunishment.Term);
      Assert.AreEqual(expectedType, autoPunishment.Type);
      Assert.AreEqual(expectedDuration, autoPunishment.Duration);
    }

    [TestMethod]
    public void ModCommandRepositoryLogic_AddMuteWithExistingMute_UpdatesDb() {
      var mutedPhrase = "words are wind";
      var expectedType = AutoPunishmentType.MutedString;
      var firstDuration = TimeSpan.FromSeconds(1);
      var expectedDuration = TimeSpan.FromSeconds(2);
      var testContainerManager = new TestContainerManager().InitializeAndIsolateRepository();
      var modCommandLogic = testContainerManager.GetInstance<IModCommandRepositoryLogic>();
      var unitOfWork = testContainerManager.GetInstance<IQueryCommandService<IUnitOfWork>>();
      AddMute(modCommandLogic, mutedPhrase, firstDuration, unitOfWork);

      modCommandLogic.AddMute(mutedPhrase, expectedDuration);

      var autoPunishments = unitOfWork.Query(u => u.AutoPunishments.GetAllWithUser);
      var autoPunishment = autoPunishments.Single();
      Assert.AreEqual(mutedPhrase, autoPunishment.Term);
      Assert.AreEqual(expectedType, autoPunishment.Type);
      Assert.AreEqual(expectedDuration, autoPunishment.Duration);
    }

    [TestMethod]
    public void ModCommandRepositoryLogic_DelMuteWithExistingMute_DeletesFromDb() {
      var mutedPhrase = "words are wind";
      var mutedDuration = TimeSpan.FromSeconds(1);
      var testContainerManager = new TestContainerManager().InitializeAndIsolateRepository();
      var modCommandLogic = testContainerManager.GetInstance<IModCommandRepositoryLogic>();
      var unitOfWork = testContainerManager.GetInstance<IQueryCommandService<IUnitOfWork>>();
      AddMute(modCommandLogic, mutedPhrase, mutedDuration, unitOfWork);

      modCommandLogic.DelMute(mutedPhrase);

      var autoPunishments = unitOfWork.Query(u => u.AutoPunishments.GetAllWithUser);
      Assert.IsFalse(autoPunishments.Any());
    }

    [TestMethod]
    public void ModCommandRepositoryLogic_DelMuteWithEmptyDb_YieldsEmptyDb() {
      var mutedPhrase = "words are wind";
      var testContainerManager = new TestContainerManager().InitializeAndIsolateRepository();
      var modCommandLogic = testContainerManager.GetInstance<IModCommandRepositoryLogic>();
      var unitOfWork = testContainerManager.GetInstance<IQueryCommandService<IUnitOfWork>>();

      modCommandLogic.DelMute(mutedPhrase);

      var autoPunishments = unitOfWork.Query(u => u.AutoPunishments.GetAllWithUser);
      Assert.IsFalse(autoPunishments.Any());
    }

    [TestMethod]
    public void ModCommandRepositoryLogic_AddMuteEmptyDb_AddedToAutomuteListFor1h() {
      var mutedPhrase = "words are wind";
      var duration = TimeSpan.FromHours(1);
      var expectedMessage = $"{mutedPhrase} added to automute list for 1h";
      var testContainerManager = new TestContainerManager().InitializeAndIsolateRepository();
      var modCommandLogic = testContainerManager.GetInstance<IModCommandRepositoryLogic>();

      var sendables = modCommandLogic.AddMute(mutedPhrase, duration);

      var actualMessage = sendables.Cast<SendablePublicMessage>().Single().Text;
      Assert.AreEqual(expectedMessage, actualMessage);
    }

    [TestMethod]
    public void ModCommandRepositoryLogic_AddMuteWithExistingMute_IsAlreadyInTheAutomuteList() {
      var mutedPhrase = "words are wind";
      var firstDuration = TimeSpan.FromSeconds(1);
      var secondDuration = TimeSpan.FromHours(2);
      var expectedMessage = $"{mutedPhrase} is already in the automute list. Its duration has been updated to 2h";
      var testContainerManager = new TestContainerManager().InitializeAndIsolateRepository();
      var modCommandLogic = testContainerManager.GetInstance<IModCommandRepositoryLogic>();
      var unitOfWork = testContainerManager.GetInstance<IQueryCommandService<IUnitOfWork>>();
      AddMute(modCommandLogic, mutedPhrase, firstDuration, unitOfWork);

      var sendables = modCommandLogic.AddMute(mutedPhrase, secondDuration);

      var actualMessage = sendables.Cast<SendablePublicMessage>().Single().Text;
      Assert.AreEqual(expectedMessage, actualMessage);
    }

    [TestMethod]
    public void ModCommandRepositoryLogic_DelMuteWithExistingMute_DeletedFromTheAutomutelist() {
      var mutedPhrase = "words are wind";
      var mutedDuration = TimeSpan.FromSeconds(1);
      var expectedMessage = $"{mutedPhrase} deleted from the automute list";
      var testContainerManager = new TestContainerManager().InitializeAndIsolateRepository();
      var modCommandLogic = testContainerManager.GetInstance<IModCommandRepositoryLogic>();
      var unitOfWork = testContainerManager.GetInstance<IQueryCommandService<IUnitOfWork>>();
      AddMute(modCommandLogic, mutedPhrase, mutedDuration, unitOfWork);

      var sendables = modCommandLogic.DelMute(mutedPhrase);

      var actualMessage = sendables.Cast<SendablePublicMessage>().Single().Text;
      Assert.AreEqual(expectedMessage, actualMessage);
    }

    [TestMethod]
    public void ModCommandRepositoryLogic_DelMuteWithEmptyDb_IsNotInTheAutomuteList() {
      var mutedPhrase = "words are wind";
      var expectedMessage = $"{mutedPhrase} is not in the automute list";
      var testContainerManager = new TestContainerManager().InitializeAndIsolateRepository();
      var modCommandLogic = testContainerManager.GetInstance<IModCommandRepositoryLogic>();

      var sendables = modCommandLogic.DelMute(mutedPhrase);

      var actualMessage = sendables.Cast<SendablePublicMessage>().Single().Text;
      Assert.AreEqual(expectedMessage, actualMessage);
    }

  }
}
