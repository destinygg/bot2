﻿using System.Threading.Tasks.Dataflow;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface IReceivedProducer {
    void Run(IReceivedProcessor receivedProcessor);
    ISourceBlock<IReceived> Produce { get; }
  }
}