using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class PunishmentFactoryTests {

    private Container GetContainer(List<SendableMute> punishments) {
      var anyPunishmentFactory = Substitute.For<IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>>();
      anyPunishmentFactory.Create(Arg.Any<ISnapshot<Civilian, PublicMessage>>()).Returns(punishments);
      return new TestContainerManager(c => {
        var repositoryPunishmentFactoryRegistration = Lifestyle.Singleton.CreateRegistration(() => anyPunishmentFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>>), repositoryPunishmentFactoryRegistration, s => s.Consumer.Target.Name == "repositoryPunishmentFactory");
      }).Container;
    }

    [TestMethod]
    public void PunishmentFactory_NoPunishments_YieldsNothing() {
      var inputPunishments = new List<SendableMute>();
      var container = GetContainer(inputPunishments);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      var punishments = punishmentFactory.Create(Arg.Any<ISnapshot<Civilian, PublicMessage>>());

      Assert.AreEqual(0, punishments.Count);
    }

    [TestMethod]
    public void PunishmentFactory_OnePunishment_YieldsPunishmentAndMessage() {
      var reason = "A made up reason";
      var inputPunishments = new List<SendableMute> {
        new SendableMute("", TimeSpan.FromTicks(1), reason)
      };
      var container = GetContainer(inputPunishments);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      var punishments = punishmentFactory.Create(Arg.Any<ISnapshot<Civilian, PublicMessage>>());

      var punishmentCount = punishments.OfType<SendableMute>().Count();
      Assert.AreEqual(1, punishmentCount);
      var message = punishments.OfType<SendablePublicMessage>().Single();
      Assert.AreEqual(reason, message.Text);
    }

    [TestMethod]
    public void PunishmentFactory_TwoPunishments_A_YieldsMaximumPunishmentAndMessage() {
      var shortReason = "Short reason";
      var longReason = "Long reason";
      var inputPunishments = new List<SendableMute> {
        new SendableMute("", TimeSpan.FromTicks(1), shortReason),
        new SendableMute("", TimeSpan.FromDays(1), longReason)
      };
      var container = GetContainer(inputPunishments);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      var punishments = punishmentFactory.Create(Arg.Any<ISnapshot<Civilian, PublicMessage>>());

      var punishmentCount = punishments.OfType<SendableMute>().Count();
      Assert.AreEqual(1, punishmentCount);
      var message = punishments.OfType<SendablePublicMessage>().Single();
      Assert.AreEqual(longReason, message.Text);
    }

    [TestMethod]
    public void PunishmentFactory_TwoPunishments_B_YieldsMaximumPunishmentAndMessage() {
      var shortReason = "Short reason";
      var longReason = "Long reason";
      var inputPunishments = new List<SendableMute> {
        new SendableMute("", TimeSpan.FromDays(1), longReason),
        new SendableMute("", TimeSpan.FromTicks(1), shortReason)
      };
      var container = GetContainer(inputPunishments);
      var punishmentFactory = container.GetInstance<PunishmentFactory>();

      var punishments = punishmentFactory.Create(Arg.Any<ISnapshot<Civilian, PublicMessage>>());

      var punishmentCount = punishments.OfType<SendableMute>().Count();
      Assert.AreEqual(1, punishmentCount);
      var message = punishments.OfType<SendablePublicMessage>().Single();
      Assert.AreEqual(longReason, message.Text);
    }

  }
}
