using System.Linq;
using System.Text.RegularExpressions;

namespace Bot.Models.Interfaces {
  public static class IReceivedMessageExtensionMethods {
    public static bool StartsWith(this IReceived<IUser, IMessage> message, params string[] phrases) => phrases.Any(p => message.Transmission.Text.StartsWith(p, System.StringComparison.InvariantCultureIgnoreCase));
    public static bool IsMatch(this IReceived<IUser, IMessage> message, Regex regex) => regex.IsMatch(message.Transmission.Text);

  }
}
