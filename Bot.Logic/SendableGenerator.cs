using System.Collections.Generic;
using Bot.Logic.Interfaces;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class SendableGenerator : ISendableGenerator {
    private readonly ILogger _logger;
    private readonly IUserVisitor<IReceivedVisitor> _userVisitor;

    public SendableGenerator(ILogger logger, IUserVisitor<IReceivedVisitor> userVisitor) {
      _logger = logger;
      _userVisitor = userVisitor;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Generate(ISnapshot<IUser, ITransmittable> snapshot) {
      _logger.LogVerbose(snapshot.Latest.ToString());
      var receivedVisitor = snapshot.Latest.Sender.Accept(_userVisitor);
      var func = snapshot.Latest.Accept(receivedVisitor);
      return func(snapshot);
    }

  }
}
