using Bot.Models.Contracts;

namespace Bot.Models {
  public class PublicMessage : IPublicMessage {
    public PublicMessage(string text) {
      Text = text;
    }

    // To ensure thread safety, this object should remain readonly.
    public string Text { get; }
    public string ConsolePrint => $"Sending a public message: {Text}";

  }
}
