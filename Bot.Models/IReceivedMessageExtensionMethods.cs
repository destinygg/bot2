using System.Text.RegularExpressions;
using Bot.Models.Contracts;

namespace Bot.Models {
  public static class IReceivedMessageExtensionMethods {
    public static bool StartsWith(this IReceivedMessage<IUser> message, string phrase) => message.Text.StartsWith(phrase);
    public static bool IsMatch(this IReceivedMessage<IUser> message, Regex regex) => regex.IsMatch(message.Text);

  }
}
