using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class UnMuteBan : ITargetable {
    protected UnMuteBan(IUser target) {
      Target = target;
    }

    public IUser Target { get; }
  }
}
