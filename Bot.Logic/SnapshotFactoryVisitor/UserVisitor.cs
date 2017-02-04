﻿using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Microsoft.CSharp.RuntimeBinder;

namespace Bot.Logic.SnapshotFactoryVisitor {
  public abstract class UserVisitor<TUser> : IReceivedVisitor<SnapshotFactory>
    where TUser : IUser {
    private readonly ILogger _logger;
    private readonly ITimeService _timeService;

    protected UserVisitor(ILogger logger, ITimeService timeService) {
      _logger = logger;
      _timeService = timeService;
    }

    public SnapshotFactory Visit<TVisitedUser, TTransmission>(Received<TVisitedUser, TTransmission> received) // todo: TVisitedUser should be the same as TUser
      where TVisitedUser : IUser
      where TTransmission : ITransmittable {
      try {
        return DynamicVisit(received as dynamic);
      } catch (RuntimeBinderException e) {
        _logger.LogError("\n===   Begin Error   ===");
        _logger.LogError($"UserVisitor did not handle this type: {received.GetType()}");
        _logger.LogError(e.Message);
        _logger.LogError(e.StackTrace);
        _logger.LogError("===   End Error   ===\n");
        return new SnapshotFactory(s => new Snapshot<IUser, ITransmittable>(new PublicMessageFromMod("Placeholder for an error", _timeService.UtcNow), new List<IReceived<IUser, ITransmittable>>()));
      }
    }

    protected abstract SnapshotFactory DynamicVisit(Received<TUser, PublicMessage> received);

    protected SnapshotFactory NewSnapshotFactory(Func<IReadOnlyList<IReceived<IUser, ITransmittable>>, ISnapshot<IUser, ITransmittable>> create) => new SnapshotFactory(create);
  }
}
