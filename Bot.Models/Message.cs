using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class Message : IMessage {
    protected Message(string text) {
      Text = text;
    }

    // To ensure thread safety, this object should remain readonly.
    public string Text { get; }

  }
}
