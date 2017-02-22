using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Tools;
using Bot.Tools.Logging;
using Microsoft.CSharp.RuntimeBinder;

namespace Bot.Logic.SendableVisitor {
  public class ConsoleSendableVisitor : ISendableVisitor<string> {
    private readonly ILogger _logger;

    public ConsoleSendableVisitor(ILogger logger) {
      _logger = logger;
    }

    public string Visit(ISendable<PublicMessage> publicMessage) => $"PublicMessage -> {publicMessage.Transmission.Text}";

    public string Visit(ISendable<ErrorMessage> error) => $"Error: -> {error.Transmission.Text}";

    public string Visit(ISendable<Pardon> pardon) => $"Pardon -> {pardon.Transmission.Target}";

    public string Visit(ISendable<Mute> mute) => $"Mute -> {mute.Transmission.Target} for {mute.Transmission.Duration.ToPretty(_logger)} Reason: {mute.Transmission.Reason}";
  }
}
