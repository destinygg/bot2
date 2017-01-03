using System.Text.RegularExpressions;

namespace Bot.Models.Contracts {
  public interface IMessage {
    string Text { get; }
    bool StartsWith(string phrase);
    bool IsMatch(Regex regex);
  }
}
