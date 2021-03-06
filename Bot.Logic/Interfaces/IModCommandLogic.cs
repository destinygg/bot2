﻿using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IModCommandLogic {
    ISendable<PublicMessage> Long(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
    ISendable<PublicMessage> Sing();
    IReadOnlyList<ISendable<ITransmittable>> Nuke(IReadOnlyList<IReceived<IUser, ITransmittable>> context, Nuke nuke);
    IReadOnlyList<ISendable<ITransmittable>> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
    IReadOnlyList<ISendable<ITransmittable>> Stalk(string user);
    IReadOnlyList<ISendable<ITransmittable>> Ipban(string nick, TimeSpan duration);
    IReadOnlyList<ISendable<ITransmittable>> Ban(string nick, TimeSpan duration);
    IReadOnlyList<ISendable<ITransmittable>> Mute(string nick, TimeSpan duration);
    IReadOnlyList<ISendable<ITransmittable>> Pardon(string nick);
  }
}
