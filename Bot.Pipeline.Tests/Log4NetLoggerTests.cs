using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bot.Logic;
using Bot.Main.Moderate;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class Log4NetLoggerTests {

    [TestMethod]
    public void Log4NetLogger_Displays_Exceptions() {
      XmlConfigurator.Configure(new FileInfo(@"log4net.config"));
      var containerManager = new TestContainerManager();
      var log4NetLogger = containerManager.Container.GetInstance<Log4NetLogger<Log4NetLoggerTests>>();
      try {
        var zero = 0;
        var undefined = 1 / zero;
      } catch (Exception e) {
        log4NetLogger.LogError("An error message", e);
      }
    }

    [TestMethod]
    public void Log4NetLogger_Displays_TryCatchFactoryExceptions() {
      XmlConfigurator.Configure(new FileInfo(@"log4net.config"));
      var containerManager = new TestContainerManager();
      var errorableFactory = containerManager.Container.GetInstance<IErrorableFactory<ISnapshot<Moderator, IMessage>, IReadOnlyList<ISendable<ITransmittable>>>>();
      var receivedFactory = containerManager.Container.GetInstance<ReceivedFactory>();
      var snapshotFactory = containerManager.Container.GetInstance<SnapshotFactory>();
      var snapshot = snapshotFactory.Create(receivedFactory.ModPublicReceivedMessage("derp"));
      errorableFactory.Create((ISnapshot<Moderator, PublicMessage>) snapshot);
    }

    [TestMethod]
    public void Log4NetLogger_Displays_TryCatchFactoryExceptionsWithTwoArguments() {
      XmlConfigurator.Configure(new FileInfo(@"log4net.config"));
      var civ = Substitute.For<IReceived<Civilian, PublicMessage>>();
      var containerManager = new TestContainerManager();
      var errorableFactory = containerManager.Container.GetInstance<IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>>>();
      var receivedFactory = containerManager.Container.GetInstance<ReceivedFactory>();
      var nukeMessage = receivedFactory.ModPublicReceivedMessage("!nuke everything");
      var nukeFactory = containerManager.Container.GetInstance<NukeFactory>();
      var nuke = nukeFactory.Create(nukeMessage);
      errorableFactory.Create(nuke, civ.Wrap().ToList());
    }

  }
}
