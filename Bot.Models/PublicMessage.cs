using Bot.Models.Contracts;

namespace Bot.Models {
  public class PublicMessage : IPublicMessage {
    public PublicMessage(string text) {
      Text = text;
    }

    public string Text { get; }
  }
}
