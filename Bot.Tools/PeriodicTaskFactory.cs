﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class PeriodicTaskFactory : IFactory<TimeSpan, Action, Task> {

    public async Task Create(TimeSpan period, Action action) =>
      await Run(action, period, CancellationToken.None);

    private async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken) {
      while (!cancellationToken.IsCancellationRequested) {
        action();
        await Task.Delay(period, cancellationToken);
      }

    }

  }
}
