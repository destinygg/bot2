using System.Collections.Generic;
using System.Linq;
using Bot.Logic;
using Bot.Logic.SnapshotFactoryVisitor;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools;

namespace Bot.Pipeline {
  public class ReceivedToSnapshot : IReceivedToSnapshot {
    private readonly IUserVisitor<IReceivedVisitor<SnapshotFactory>> _userVisitor;
    private readonly List<IReceived<IUser, ITransmittable>> _context = new List<IReceived<IUser, ITransmittable>>(); // Todo: Optimization: Use a circular buffer

    public ReceivedToSnapshot(IUserVisitor<IReceivedVisitor<SnapshotFactory>> userVisitor) {
      _userVisitor = userVisitor;
    }

    public ISnapshot<IUser, ITransmittable> GetSnapshot(IReceived<IUser, ITransmittable> first) {
      try {
        var receivedVisitor = first.Sender.Accept(_userVisitor);
        var snapshotFactory = first.Accept(receivedVisitor);
        return snapshotFactory.Create(_context.ToList()); // The .ToList() creates a new instance; important!
      } finally {
        _context.Insert(0, first);
        if (_context.Count > Settings.ContextSize) {
          _context.RemoveAt(Settings.ContextSize);
        }
      }
    }

  }
}
