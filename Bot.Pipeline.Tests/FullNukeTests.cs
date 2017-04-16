using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Bot.Pipeline.Tests {
  [TestClass]
  public class FullNukeTests {

    [TestMethod]
    public void NukePunishes_UsersAfter_Always() {
      var sender = new TestableSender();
      var containerManager = new TestContainerManager(container => {
        var timeServiceRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, container);
        container.RegisterConditional(typeof(ICommandHandler<IEnumerable<ISendable<ITransmittable>>>), timeServiceRegistration, _ => true);
      }).InitializeAndIsolateRepository();
      var factory = containerManager.GetInstance<ReceivedFactory>();
      var pipeline = containerManager.GetInstance<IPipeline>();
      var data = new List<IReceived<IUser, ITransmittable>> {
        factory.ModPublicReceivedMessage("!nuke banplox"),
        factory.PublicReceivedMessage("User", "banplox"),
      };

      data.ForEach(x => {
        Task.Delay(1000).Wait();
        pipeline.Enqueue(x);
      });

      Task.Delay(1000).Wait();
      Assert.IsTrue(sender.Outbox.Cast<SendableMute>().First().Target.Nick == "User");
    }

  }
}
