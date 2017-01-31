using System.Collections.Generic;
using Bot.Logic.Interfaces;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;

namespace Bot.Pipeline {
  public class SnapshotToSendable : ISnapshotToSendable {
    private readonly ISendableGenerator _sendableGenerator;

    public SnapshotToSendable(ISendableGenerator sendableGenerator) {
      _sendableGenerator = sendableGenerator;
    }

    public IReadOnlyList<ISendable> GetSendables(ISnapshot<IUser, ITransmittable> snapshot) => _sendableGenerator.Generate(snapshot);
  }
}
