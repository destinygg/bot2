using System.Text.RegularExpressions;

namespace Bot.Models.Interfaces {
  public static class IReceivedMessageExtensionMethods {
    public static bool StartsWith(this IReceivedMessage<IUser> message, string phrase) => message.Text.StartsWith(phrase);
    public static bool IsMatch(this IReceivedMessage<IUser> message, Regex regex) => regex.IsMatch(message.Text);

  }
}
