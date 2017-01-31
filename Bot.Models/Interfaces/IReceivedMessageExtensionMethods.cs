using System.Text.RegularExpressions;

namespace Bot.Models.Interfaces {
  public static class IReceivedMessageExtensionMethods {
    public static bool StartsWith(this IReceived<IUser, IMessage> message, string phrase) => message.Transmission.Text.StartsWith(phrase);
    public static bool IsMatch(this IReceived<IUser, IMessage> message, Regex regex) => regex.IsMatch(message.Transmission.Text);

  }
}
