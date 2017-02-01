using Bot.Models.Interfaces;

namespace Bot.Models {
  public class Pardon : ITargetable {
    public Pardon(IUser target) {
      Target = target;
    }

    public IUser Target { get; }
  }
}
