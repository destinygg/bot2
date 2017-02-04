using System;
using System.Collections.Generic;
using Bot.Logic.Interfaces;
using Bot.Logic.SendablesFactoryVisitor;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class SendableGenerator : ISendableGenerator {
    private readonly ILogger _logger;
    private readonly IUserVisitor<IReceivedVisitor<SendablesFactory>> _userVisitor;

    public SendableGenerator(ILogger logger, IUserVisitor<IReceivedVisitor<SendablesFactory>> userVisitor) {
      _logger = logger;
      _userVisitor = userVisitor;
    }

    public IReadOnlyList<ISendable<ITransmittable>> Generate(ISnapshot<IUser, ITransmittable> snapshot) {
      _logger.LogVerbose(snapshot.Latest.ToString());
      var receivedVisitor = snapshot.Latest.Sender.Accept(_userVisitor);
      var sendablesFactory = snapshot.Latest.Accept(receivedVisitor);
      return sendablesFactory.Create(snapshot);
    }

  }
}
