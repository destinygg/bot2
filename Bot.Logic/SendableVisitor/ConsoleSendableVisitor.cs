using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Microsoft.CSharp.RuntimeBinder;

namespace Bot.Logic.SendableVisitor {
  public class ConsoleSendableVisitor : ISendableVisitor<string> {
    private readonly ILogger _logger;

    public ConsoleSendableVisitor(ILogger logger) {
      _logger = logger;
    }

    public string Visit<TTransmission>(ISendable<TTransmission> sendable)
      where TTransmission : ITransmittable => DynamicVisit(sendable as dynamic) ?? "";

    public string DynamicVisit(dynamic sendable) {
      try {
        return _DynamicVisit(sendable);
      } catch (RuntimeBinderException e) {
        _logger.LogError($"{nameof(SnapshotVisitor.BaseSnapshotVisitor<IUser>)} did not handle this type: {sendable.GetType()}", e);
        return null;
      }
    }

    private string _DynamicVisit(ISendable<PublicMessage> publicMessage) => $"PublicMessage -> {publicMessage.Transmission.Text}";

    private string _DynamicVisit(ISendable<PrivateMessage> privateMessage) => $"PrivateMessage -> {privateMessage.Transmission.Text}";

    private string _DynamicVisit(ISendable<Mute> mute) => $"Mute -> {mute.Transmission.Target} for {mute.Transmission.Duration.ToPretty(_logger)} Reason: {mute.Transmission.Reason}";

    private string _DynamicVisit(ISendable<Pardon> pardon) => $"Pardon -> {pardon.Transmission.Target}";
  }
}
