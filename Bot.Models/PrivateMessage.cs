using Bot.Models.Interfaces;

namespace Bot.Models {
  public class PrivateMessage : Message, ITargetable {
    public PrivateMessage(string text, IUser target) : base(text) {
      Target = target;
    }

    public IUser Target { get; }
  }
}
