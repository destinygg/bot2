using System;
using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Microsoft.CSharp.RuntimeBinder;

namespace Bot.Logic.ReceivedVisitor {
  public abstract class BaseReceivedVisitor<TUser> : IReceivedVisitor<DelegatedSnapshotFactory>
    where TUser : IUser {
    private readonly ILogger _logger;
    private readonly ITimeService _timeService;

    protected BaseReceivedVisitor(ILogger logger, ITimeService timeService) {
      _logger = logger;
      _timeService = timeService;
    }

    public DelegatedSnapshotFactory Visit<TVisitedUser, TTransmission>(Received<TVisitedUser, TTransmission> received) // todo: TVisitedUser should be the same as TUser
      where TVisitedUser : IUser
      where TTransmission : ITransmittable {
      try {
        return DynamicVisit(received as dynamic);
      } catch (RuntimeBinderException e) {
        _logger.LogError(e, $"{nameof(BaseReceivedVisitor<IUser>)} did not handle this type: {received.GetType()}");
        return new DelegatedSnapshotFactory(s => new Snapshot<IUser, ITransmittable>(new ReceivedError("Placeholder for an error", _timeService.UtcNow), new List<IReceived<IUser, ITransmittable>>()));
      }
    }

    protected abstract DelegatedSnapshotFactory DynamicVisit(Received<TUser, PublicMessage> received);

    protected DelegatedSnapshotFactory NewSnapshotFactory(Func<IReadOnlyList<IReceived<IUser, ITransmittable>>, ISnapshot<IUser, ITransmittable>> create) => new DelegatedSnapshotFactory(create);
  }
}
