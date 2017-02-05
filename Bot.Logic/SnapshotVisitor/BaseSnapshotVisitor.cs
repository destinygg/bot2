﻿using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Microsoft.CSharp.RuntimeBinder;

namespace Bot.Logic.SnapshotVisitor {
  public abstract class BaseSnapshotVisitor<TUser> : ISnapshotVisitor<IReadOnlyList<ISendable<ITransmittable>>>
    where TUser : IUser {
    private readonly ILogger _logger;

    protected BaseSnapshotVisitor(ILogger logger) {
      _logger = logger;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Visit<TVisitedUser, TTransmission>(ISnapshot<TVisitedUser, TTransmission> received) // todo: TVisitedUser should be the same as TUser
      where TVisitedUser : IUser
      where TTransmission : ITransmittable {
      try {
        return DynamicVisit(received as dynamic);
      } catch (RuntimeBinderException e) {
        _logger.LogError(e, $"{nameof(BaseSnapshotVisitor<IUser>)} did not handle this type: {received.GetType()}");
        return new List<ISendable<ITransmittable>>();
      }
    }

    protected abstract IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<TUser, PublicMessage> snapshot);
    protected abstract IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<TUser, PrivateMessage> snapshot);

    private IReadOnlyList<ISendable<ITransmittable>> DynamicVisit(ISnapshot<IUser, ITransmittable> snapshot) =>
      new List<ISendable<ITransmittable>>();
  }
}