using System.Text.RegularExpressions;

namespace Bot.Models.Contracts {
  public static class IMessageExtensionMethods {
    public static bool StartsWith(this IMessage message, string phrase) => message.Text.StartsWith(phrase);
    public static bool IsMatch(this IMessage message, Regex regex) => regex.IsMatch(message.Text);

  }
}
