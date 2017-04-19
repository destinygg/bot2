using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Pipeline.Tests;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Main.Moderate.Tests {
  [TestClass]
  public class PeriodicTasksTests {

    [TestMethod]
    public void PeriodicTasks_Run_YieldsAlternatingMessages() {
      var sender = new TestableSender();
      var container = new TestContainerManager(c => {
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, c);
        c.RegisterConditional(typeof(ICommandHandler<IEnumerable<ISendable<ITransmittable>>>), timeServiceRegistration, _ => true);
      }, settings => settings.PeriodicTaskInterval = TimeSpan.FromMilliseconds(100))
      .InitializeAndIsolateRepository();
      var tasks = container.GetInstance<PeriodicTasks>();

      tasks.Run();

      Task.Delay(1000).Wait();
      Assert.AreEqual(5, sender.Outbox.Cast<SendablePublicMessage>().Count(x => x.Text.Contains("GreenManGaming")));
      Assert.AreEqual(5, sender.Outbox.Cast<SendablePublicMessage>().Count(x => x.Text.Contains("twitter.com")));
      Assert.AreEqual(10, sender.Outbox.Count);
    }

  }
}
