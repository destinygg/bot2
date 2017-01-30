﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;
using Bot.Tools;

namespace Bot.Pipeline {
  public class ReceivedToContextualized : IReceivedToContextualized {

    private readonly List<IReceived<IUser, ITransmittable>> context = new List<IReceived<IUser, ITransmittable>>(); // Todo: Optimization: Use a circular buffer

    public IContextualized<IUser, ITransmittable> GetContextualized(IReceived<IUser, ITransmittable> first) {
      try {
        return new Contextualized<IUser, ITransmittable>(first, context.ToList()); // The .ToList() creates a new instance; important!
      } finally {
        context.Insert(0, first);
        if (context.Count > Settings.ContextSize) {
          context.RemoveAt(Settings.ContextSize);
        }
      }
    }

  }
}
