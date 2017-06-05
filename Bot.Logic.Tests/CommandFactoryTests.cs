using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Main.Moderate;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class CommandFactoryTests {

    private Container CreateContainer(ICommandLogic commandLogic) {
      return new TestContainerManager(c => {
        var commandLogicRegistration = Lifestyle.Singleton.CreateRegistration(() => commandLogic, c);
        c.RegisterConditional(typeof(ICommandLogic), commandLogicRegistration, pc => !pc.Handled);
      }).InitializeAndIsolateRepository();
    }

    [TestMethod]
    public void CommandFactory_i_ASLAN_Succeeds() {
      var expectedResponse = "meow";
      var commandLogic = Substitute.For<ICommandLogic>();
      commandLogic.TwitterAslan().Returns(new SendablePublicMessage(expectedResponse).Wrap().ToList());
      var container = CreateContainer(commandLogic);
      var commandFactory = container.GetInstance<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var publicMessageFromCivilianSnapshot = receivedFactory.PublicReceivedSnapshot("! ASLAN");

      var response = commandFactory.Create(publicMessageFromCivilianSnapshot);

      Assert.AreEqual(expectedResponse, response.OfType<SendablePublicMessage>().Single().Transmission.Text);
    }

    [TestMethod]
    public void CommandFactory_iASLAN_Succeeds() {
      var expectedResponse = "meow";
      var commandLogic = Substitute.For<ICommandLogic>();
      commandLogic.TwitterAslan().Returns(new SendablePublicMessage(expectedResponse).Wrap().ToList());
      var container = CreateContainer(commandLogic);
      var commandFactory = container.GetInstance<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var publicMessageFromCivilianSnapshot = receivedFactory.PublicReceivedSnapshot("!ASLAN");

      var response = commandFactory.Create(publicMessageFromCivilianSnapshot);

      Assert.AreEqual(expectedResponse, response.OfType<SendablePublicMessage>().Single().Transmission.Text);
    }

    [TestMethod]
    public void CommandFactory_iaslan_Succeeds() {
      var expectedResponse = "meow";
      var commandLogic = Substitute.For<ICommandLogic>();
      commandLogic.TwitterAslan().Returns(new SendablePublicMessage(expectedResponse).Wrap().ToList());
      var container = CreateContainer(commandLogic);
      var commandFactory = container.GetInstance<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var publicMessageFromCivilianSnapshot = receivedFactory.PublicReceivedSnapshot("!aslan");

      var response = commandFactory.Create(publicMessageFromCivilianSnapshot);

      Assert.AreEqual(expectedResponse, response.OfType<SendablePublicMessage>().Single().Transmission.Text);
    }

    [TestMethod]
    public void CommandFactory_i_aslan_Succeeds() {
      var expectedResponse = "meow";
      var commandLogic = Substitute.For<ICommandLogic>();
      commandLogic.TwitterAslan().Returns(new SendablePublicMessage(expectedResponse).Wrap().ToList());
      var container = CreateContainer(commandLogic);
      var commandFactory = container.GetInstance<IErrorableFactory<ISnapshot<IUser, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>>();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var publicMessageFromCivilianSnapshot = receivedFactory.PublicReceivedSnapshot("! aslan");

      var response = commandFactory.Create(publicMessageFromCivilianSnapshot);

      Assert.AreEqual(expectedResponse, response.OfType<SendablePublicMessage>().Single().Transmission.Text);
    }

  }
}
