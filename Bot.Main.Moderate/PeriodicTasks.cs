using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Main.Moderate {
  public class PeriodicTasks {
    private readonly ICommandHandler<IEnumerable<ISendable<ITransmittable>>> _sender;
    private readonly ISettings _settings;
    private readonly List<IEnumerable<SendablePublicMessage>> _periodicMessages;

    public PeriodicTasks(ICommandHandler<IEnumerable<ISendable<ITransmittable>>> sender, IQueryCommandService<IUnitOfWork> unitOfWork, ISettings settings) {
      _periodicMessages = unitOfWork.Query(u => u.PeriodicMessages.GetAll).Select(x => new SendablePublicMessage(x).Wrap()).ToList();
      _sender = sender;
      _settings = settings;
    }

    public void Run() {
      var periodicTaskFactory = new PeriodicTaskFactory();
      var rng = new Random();
      var i = rng.Next(_periodicMessages.Count);
      periodicTaskFactory.Create(_settings.PeriodicTaskInterval, () => {
        _sender.Handle(_periodicMessages[i]);
        i++;
        if (i == _periodicMessages.Count) {
          i = 0;
        }
      });
    }

  }
}
