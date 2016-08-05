﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Client.Contracts;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ReceivedProcessor : IReceivedProcessor {
    private readonly IBanLogic _banLogic;
    private readonly ISender _sender;

    public ReceivedProcessor(IBanLogic banLogic) {
      _banLogic = banLogic;
    }

    public ReceivedProcessor(ISender sender) {
      _sender = sender;
    }

    public void Process(IPrivateMessageReceived privateMessageReceived) {

    }

    public void Process(IPublicMessageReceived publicMessageReceived) {
      var confirm = new PublicMessage($"Processor processing a public message: {publicMessageReceived.Text}");
      _sender.Send(confirm);
      var outbox = _banLogic.Process(publicMessageReceived);
      foreach (var outobject in outbox) {
        _sender.Send(outobject);
      }
    }

    public void Process(IMuteReceived muteReceived) {

    }

    public void Process(IUnMuteBanReceived unMuteBanReceived) {

    }

    public void Process(ISubonlyReceived subonlyReceived) {

    }

    public void Process(IBanReceived banReceived) {

    }

    public void Process(IBroadcastReceived broadcastReceived) {

    }
  }
}
