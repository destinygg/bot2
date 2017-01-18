using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class Pardon : ITargetable {
    protected Pardon(IUser target) {
      Target = target;
    }

    public IUser Target { get; }
  }
}
