using System.Text.RegularExpressions;
using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class Message : IMessage {
    protected Message(string text) {
      Text = text;
    }

    // To ensure thread safety, this object should remain readonly.
    public string Text { get; }
    public bool StartsWith(string phrase) => Text.StartsWith(phrase);
    public bool IsMatch(Regex regex) => regex.IsMatch(Text);
    public string ConsolePrint => $"Sending a public message: {Text}";
  }
}
