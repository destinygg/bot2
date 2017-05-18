using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {

  //todo find a way to apply this to CTRL V as well

  public class SingleLineSpamPunishmentFactory : IErrorableFactory<ISnapshot<Civilian, PublicMessage>, IReadOnlyList<ISendable<ITransmittable>>> {
    private readonly ISettings _settings;

    public SingleLineSpamPunishmentFactory(ISettings settings) {
      _settings = settings;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Create(ISnapshot<Civilian, PublicMessage> snapshot) {
      var punishments = new List<ISendable<ITransmittable>>();
      var message = snapshot.Latest;

      // matches single characters only
      var match = new Regex($@"(.)\1{{{_settings.RepeatCharacterSpamLimit},}}").Match(message.Transmission.Text);
      if (match.Success) {
        punishments.Add(new SendableMute(message.Sender, TimeSpan.FromMinutes(10), $"Let go of that poor {match.Groups[1].Value}; 10m"));
      }
      return punishments;
    }

    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new List<ISendable<ITransmittable>>();
  }
}
