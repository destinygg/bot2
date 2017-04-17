using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Logic.Tests {
  [TestClass]
  public class ModCommandRegexTest {

    [TestMethod]
    public void ModCommandRegex_nuke_this_AlwaysMatches() {
      var testPhrase = "!nuke this";
      var modCommandRegex = new ModCommandRegex();

      var nukeRegex = modCommandRegex.Nuke;

      Assert.IsTrue(nukeRegex.IsMatch(testPhrase));
    }

    [TestMethod]
    public void ModCommandRegex_ThrowsException_YieldsSendableErrorMessage() {
      var modCommandRegex = Substitute.For<IModCommandRegex>();
      modCommandRegex.Nuke.Returns(_ => { throw new Exception(); });
      var container = new TestContainerManager(
        c => {
          var modCommandRegexRegistration = Lifestyle.Singleton.CreateRegistration(() => modCommandRegex, c);
          c.RegisterConditional(typeof(IModCommandRegex), modCommandRegexRegistration, pc => !pc.Handled);
        }).InitializeAndIsolateRepository();
      var receivedFactory = container.GetInstance<ReceivedFactory>();
      var snapshotFactory = container.GetInstance<IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>>();
      var sendableFactory = container.GetInstance<IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>>();
      var received = receivedFactory.ModPublicReceivedMessage("!nuke10m test");
      var snapshot = snapshotFactory.Create(received);
      var sendables = sendableFactory.Create(snapshot);

      var errorMessage = (ISendable<ErrorMessage>) sendables.Single();
      Assert.AreEqual("An error occured in the ModCommandFactory.", errorMessage.Transmission.Text);
    }

  }
}
