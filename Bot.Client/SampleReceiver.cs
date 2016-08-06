﻿using System;
using System.Collections.Generic;
using Bot.Client.Contracts;
using Bot.Logic.Contracts;
using Bot.Models.Contracts;

namespace Bot.Client {
  public class SampleReceiver : IReceiver {
    private readonly IEnumerable<IReceived> _received;
    private readonly ISender _sender;

    public SampleReceiver(IEnumerable<IReceived> received, ISender sender) {
      _received = received;
      _sender = sender;
    }

    public void Run(IReceivedProcessor receivedProcessor) {
      foreach (var received in _received) {
        if (received is IPublicMessageReceived) {
          var publicMessageReceived = (IPublicMessageReceived) received;
          Console.WriteLine($"Public message received: {publicMessageReceived.Text}");
          var outbox = receivedProcessor.Process(publicMessageReceived);
          foreach (var sendable in outbox) {
            _sender.Send(sendable);
          }
        }
      }
    }
  }
}
